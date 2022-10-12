using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ThousandLines_Data;
using UnityEngine.Rendering;
using Unity.VisualScripting;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        //전체적인 게임 시스템 및 시퀀스를 관리한다.

        public static ThousandLinesManager Instance { get; private set; }

        [Header("Prefabs")]
        public BaseMachine m_BaseMachine;
        public GoalMachine m_GoalMachine;
        public MaterialObject m_MaterialObject;


        public double m_money;
        public List<Machine> m_Machines    = new List<Machine>();
        public List<Machine> m_OffMachines = new List<Machine>(); //대기중인 머신

        [SerializeField]
        private List<MaterialObject> m_MaterialObjects;

        public double Money
        {
            set
            {
                this.m_money += value;
                ThousandLinesUIManager.Instance.m_moneyText.text = m_money.ToString();
            }
        }

        private void Awake()
        {
            Instance = this;
            this.m_MaterialObjects = new List<MaterialObject>();
        }

        #region Initialize

        public void Initiaize()
        {
            var machineLineDatas = AssetDataManager.GetDatas<MachineLineData>();
            ThousandLinesUIManager.Instance.Initialize(machineLineDatas);
            this.StartCoroutine(InitializeCoroutine(machineLineDatas));
        }

        private IEnumerator InitializeCoroutine(List<MachineLineData> machineLineDatas)
        {
            this.InitializeBaseMachine();
            yield return new WaitForSeconds(0.5f);

            //테이블에 활성화 된 머신에 따라서 생성한다.
            int index = 1;
            for (int i = 0; i < machineLineDatas.Count; i++)
            {
                if (machineLineDatas[i].Line_isActive != 0)
                {
                    this.InitializeMachineLine(machineLineDatas[i], index);
                    index++;
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
            }
            this.InitializeGoalMachine();
            yield return new WaitForSeconds(0.5f);

            //모든 머신이 로드 된 후 UI 활성화
            ThousandLinesUIManager.Instance.SetAcitveGameUI(true);
        }

        private void InitializeBaseMachine()
        {
            //생산머신 초기화
            BaseMachine baseMachine = Instantiate(this.m_BaseMachine, this.transform);
            baseMachine.name = this.m_BaseMachine.name;
            
            this.m_BaseMachine = baseMachine;
            this.m_BaseMachine.Index = this.m_Machines.Count;
            this.m_Machines.Add(this.m_BaseMachine);

            this.SetSortingGroup(this.m_BaseMachine.gameObject, this.m_BaseMachine.Index);
            this.m_BaseMachine.Show();
        }

        private void InitializeMachineLine(MachineLineData machineLineData, int index)
        {
            //테이블 이름에 맞는 리스소 프리팹 로드
            GameObject machineLineGameObject = Instantiate(
                Resources.Load($"{"Prefabs/MachineLine/" + machineLineData.Id}"),
                this.transform) as GameObject;

            MachineLine machineLine = machineLineGameObject.GetComponent<MachineLine>();
            machineLine.name = machineLineData.Id;
            machineLine.Index = index;

            machineLine.SetMachine(machineLineData);
            machineLine.m_Distace = machineLineData.Line_Distance;

            machineLine.transform.position = GetMachineLinePos(machineLine);
            this.m_Machines.Add(machineLine);

            this.SetSortingGroup(machineLine.gameObject, machineLine.Index);

            machineLine.Show();
        }

        private void InitializeGoalMachine()
        {
            GoalMachine goalMachine = Instantiate(this.m_GoalMachine, this.transform);
            goalMachine.name = this.m_GoalMachine.name;
            this.m_GoalMachine = goalMachine;
            this.m_GoalMachine.Index = this.m_Machines.Count;

            this.m_GoalMachine.transform.position = GetMachineLinePos(this.m_GoalMachine);
            this.m_Machines.Add(this.m_GoalMachine);

            this.SetSortingGroup(this.m_GoalMachine.gameObject, this.m_GoalMachine.Index);
            this.m_GoalMachine.Show();
        }

        private void SetSortingGroup(GameObject gameObject, int index)
        {
            if (gameObject.GetComponent<SortingGroup>() == null)
                gameObject.AddComponent<SortingGroup>();
            gameObject.GetComponent<SortingGroup>().sortingOrder = index;
        }

        public Vector2 GetMachineLinePos(Machine machine)
        {
            float xPos = machine.m_Distace;
            for (int i = 0; i < machine.Index; i++)
            {
                if (this.m_Machines[i] != null)
                {
                    xPos += this.m_Machines[i].m_Distace;
                }
            }
            return new Vector2((xPos) , machine.transform.position.y);
        }

        #endregion

        #region MachineState

        // Receive - Wait 상태에서 (자기일 끝난 후 대기) 옆에 보내줄 때
        public void MachineReceive(Machine currentMachine)
        {
            //다음 머신 존재 여부
            var nextMachine = this.FindMachine(currentMachine, true);
            if (nextMachine == null) return;

            //다음 머신이 준비 상태인지
            if (nextMachine.machineState != MachineState.READY) return;

            // 다음 머신이 해제 대기중이면
            if (nextMachine.m_isReserved)
            {
                nextMachine.SetState(MachineState.READY);
                return;
            }

            this.SetMaterialObject(nextMachine, currentMachine);

            nextMachine.SetState(MachineState.MOVE);
            currentMachine.SetState(MachineState.READY);
        }

        // Send (일거리 당겨서 가져옴)
        public void MachineSend(Machine currentMachine)
        {
            // 내가 수령할 수 있는지
            var previousMachine = this.FindMachine(currentMachine);

            //이전애가 사라짐 (이동)
            if (previousMachine == null)
            {
                //포지션 바꿔주기 전에 일부터 해야한다!
                currentMachine.SetState(MachineState.REPOSITION);
                return;
            }

            if (previousMachine.machineState != MachineState.WAIT) return;
            //전달 완료 하였고, 이전 메테리얼 수령함 준비로 상태변경
            this.SetMaterialObject(currentMachine, previousMachine);
            
            //중요 - 일거리 있으면 실행한다
            if (previousMachine.m_MaterialObject == null)
                currentMachine.SetState(MachineState.MOVE);

            previousMachine.SetState(MachineState.READY);
        }

        public void ResetRepositionMachine(Machine currentMachine, int index)
        {
            //그런데 내가 리셋 포지션 중이면 끌고 올 수 있어야함.
            //본인이 이동중인 대상일 때
            var nextMachine = this.m_Machines.Find(index + 1);
            if (nextMachine == null) return;

            if (nextMachine.machineState == MachineState.READY)
            {
                nextMachine.SetState(MachineState.REPOSITION);
            }
        }


        public void ResetReadyMachine(Machine currentMachine, int index)
        {
            //본인이 레디 일 때
            //그런데 내가 리셋 포지션 중이면 끌고 올 수 있어야함.
            var nextMachine = this.m_Machines.Find(index + 1);
            if (nextMachine == null) return;

            if ((currentMachine.machineState == MachineState.REPOSITION || 
                currentMachine.machineState == MachineState.OUT) && 
                nextMachine.machineState == MachineState.READY)
            {
                nextMachine.SetState(MachineState.REPOSITION);
            }
        }

        private void SetMaterialObject(Machine setMachine, Machine getMachine)
        {
            setMachine.m_MaterialObject = getMachine.m_MaterialObject;
            setMachine.m_MaterialObject.transform.SetParent(setMachine.transform);
            getMachine.m_MaterialObject = null;
        }

        #endregion

        #region Others

        private Machine FindMachine(Machine currentMachine, bool isPrevious = false)
        {
            if (!isPrevious) // 다음거
            {
                var findMachine = this.m_Machines.Find(currentMachine.Index - 1);
                if (findMachine != null) return findMachine;
            }
            else
            {
                var findMachine = this.m_Machines.Find(currentMachine.Index + 1);
                if (findMachine != null) return findMachine;
            }
            return null;
        }

        public void MachineListRemove(Machine machine)
        {
            this.m_OffMachines.Add(machine);
            this.m_Machines[machine.Index] = null;
        }

        public void MachineListSet(Machine machine)
        {
            for (int i = 0; i < this.m_Machines.Count; i++)
            {
                if (this.m_Machines[i] == null)
                {
                    this.m_Machines[i] = machine;
                    this.m_Machines[machine.Index] = null;
                    machine.Index = i;
                    return;
                }
            }
        }

        #endregion
    }
}







































//천개의 줄
//천개의 공정(라인) 의 뜻
//자동화 시스템에서 생산되는 원자재를
//추가 공정 및 가공함으로써 더욱 가치있는 자재로 업그레이드 후 판매하는 게임

//머신 하나당 1개의 메테리얼만 가질 수 있다. (다음 소비가 안되면 멈춤)
//생산 쿨타임이 존재한다.

// 오브젝트 풀링
// 아이템 구조 설계
// 아이템 인터렉션