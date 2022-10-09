using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using ThousandLines_Data;

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

        [Header("UI")]
        public TextMeshProUGUI m_moneyText;
        public double m_money;


        [SerializeField]
        private List<Machine> m_Machine = new List<Machine>();
        private List<MaterialObject> m_MaterialObjects;

        public double Money
        {
            set
            {
                this.m_money += value;
                this.m_moneyText.text = m_money.ToString();
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
            this.StartCoroutine(InitializeCoroutine());
        }

        private IEnumerator InitializeCoroutine()
        {
            this.InitializeBaseMachine();
            yield return new WaitForSeconds(0.5f);

            //테이블에 활성화 된 머신에 따라서 생성한다.

            var machineLineDatas = AssetDataManager.GetDatas<MachineLineData>();
            float endPos = 0;

            for (int i = 0; i < machineLineDatas.Count; i++)
            {
                if (machineLineDatas[i].Line_isActive == 0) continue; 
                this.InitializeMachineLine(machineLineDatas[i]);
                endPos += machineLineDatas[i].Line_Distance;

                yield return new WaitForSeconds(0.5f);
            }

            this.InitializeGoalMachine(endPos);
            yield return new WaitForSeconds(0.5f);
        }

        private void InitializeBaseMachine()
        {
            BaseMachine baseMachine = Instantiate(this.m_BaseMachine, this.transform);
            baseMachine.name = this.m_BaseMachine.name;
            baseMachine.transform.position = Vector3.zero;

            this.m_BaseMachine = baseMachine;

            this.m_BaseMachine.Index = this.m_Machine.Count;
            this.m_Machine.Add(this.m_BaseMachine);

            this.m_BaseMachine.Show();
        }

        private void InitializeMachineLine(MachineLineData machineLineData)
        {
            //테이블 이름에 맞는 리스소 프리팹 로드
            GameObject machineLineGameObject = Instantiate(
                Resources.Load($"{"Prefabs/MachineLine/" + machineLineData.Id}"),
                this.transform) as GameObject;

            MachineLine machineLine = machineLineGameObject.GetComponent<MachineLine>();
            machineLine.name = machineLineData.Id;

            machineLine.Index = this.m_Machine.Count;
            machineLine.transform.position = SetMachinePos(machineLine.transform, machineLine.Index -1, machineLineData.Line_Distance);

            this.m_Machine.Add(machineLine);
            machineLine.SetMachine(machineLineData);
            machineLine.Show();
        }

        private void InitializeGoalMachine(float value)
        {
            GoalMachine goalMachine = Instantiate(this.m_GoalMachine, this.transform);
            goalMachine.name = this.m_GoalMachine.name;
            this.m_GoalMachine = goalMachine;

            this.m_GoalMachine.transform.position = new Vector2(this.m_GoalMachine.transform.position.x + value, 
                                                                this.m_GoalMachine.transform.position.y);
            this.m_GoalMachine.Index = this.m_Machine.Count;
            this.m_Machine.Add(this.m_GoalMachine);

            this.m_GoalMachine.Show();
        }

        private Vector2 SetMachinePos(Transform trValue, int index, float distance)
        {
            return new Vector2((trValue.position.x) + (index * distance), trValue.position.y);
        }

        #endregion

        #region MachineState

        // 시스템 순서
        // 현 개체와 다음 개체가 Wait 상태일 경우 다음 작동 시퀀스를 발동한다.
        public void BaseMachineNext(Machine currentMachine, Action onCompleted)
        {
            //다음 머신이 정상적으로 건네받을 수 있는지 확인
            var nextMachine = m_Machine.Find(currentMachine.Index + 1);
            if (nextMachine == null) return;
            if (nextMachine.machineState != MachineState.READY) return;

            nextMachine.m_MaterialObject = currentMachine.m_MaterialObject;
            currentMachine.m_MaterialObject = null;

            nextMachine.SetState(MachineState.MOVE);

            onCompleted?.Invoke();
        }

        //이전 개체가 완료었고, 내가 완료가 되면 호출됨
        public void MachinePrevious(Machine currentMachine, Action onCompleted)
        {
            //다음 머신이 정상적으로 건네받을 수 있는지 확인
            var previousMachine = m_Machine.Find(currentMachine.Index - 1);
            if (previousMachine == null) return;
            if (previousMachine.machineState != MachineState.WAIT) return;

            currentMachine.m_MaterialObject = previousMachine.m_MaterialObject;
            previousMachine.m_MaterialObject = null;

            previousMachine.SetState(MachineState.MOVE);

            onCompleted?.Invoke();
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