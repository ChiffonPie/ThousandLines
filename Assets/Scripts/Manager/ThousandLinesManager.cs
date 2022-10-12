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
        //��ü���� ���� �ý��� �� �������� �����Ѵ�.

        public static ThousandLinesManager Instance { get; private set; }

        [Header("Prefabs")]
        public BaseMachine m_BaseMachine;
        public GoalMachine m_GoalMachine;
        public MaterialObject m_MaterialObject;


        public double m_money;
        public List<Machine> m_Machines    = new List<Machine>();
        public List<Machine> m_OffMachines = new List<Machine>(); //������� �ӽ�

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

            //���̺� Ȱ��ȭ �� �ӽſ� ���� �����Ѵ�.
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

            //��� �ӽ��� �ε� �� �� UI Ȱ��ȭ
            ThousandLinesUIManager.Instance.SetAcitveGameUI(true);
        }

        private void InitializeBaseMachine()
        {
            //����ӽ� �ʱ�ȭ
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
            //���̺� �̸��� �´� ������ ������ �ε�
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

        // Receive - Wait ���¿��� (�ڱ��� ���� �� ���) ���� ������ ��
        public void MachineReceive(Machine currentMachine)
        {
            //���� �ӽ� ���� ����
            var nextMachine = this.FindMachine(currentMachine, true);
            if (nextMachine == null) return;

            //���� �ӽ��� �غ� ��������
            if (nextMachine.machineState != MachineState.READY) return;

            // ���� �ӽ��� ���� ������̸�
            if (nextMachine.m_isReserved)
            {
                nextMachine.SetState(MachineState.READY);
                return;
            }

            this.SetMaterialObject(nextMachine, currentMachine);

            nextMachine.SetState(MachineState.MOVE);
            currentMachine.SetState(MachineState.READY);
        }

        // Send (�ϰŸ� ��ܼ� ������)
        public void MachineSend(Machine currentMachine)
        {
            // ���� ������ �� �ִ���
            var previousMachine = this.FindMachine(currentMachine);

            //�����ְ� ����� (�̵�)
            if (previousMachine == null)
            {
                //������ �ٲ��ֱ� ���� �Ϻ��� �ؾ��Ѵ�!
                currentMachine.SetState(MachineState.REPOSITION);
                return;
            }

            if (previousMachine.machineState != MachineState.WAIT) return;
            //���� �Ϸ� �Ͽ���, ���� ���׸��� ������ �غ�� ���º���
            this.SetMaterialObject(currentMachine, previousMachine);
            
            //�߿� - �ϰŸ� ������ �����Ѵ�
            if (previousMachine.m_MaterialObject == null)
                currentMachine.SetState(MachineState.MOVE);

            previousMachine.SetState(MachineState.READY);
        }

        public void ResetRepositionMachine(Machine currentMachine, int index)
        {
            //�׷��� ���� ���� ������ ���̸� ���� �� �� �־����.
            //������ �̵����� ����� ��
            var nextMachine = this.m_Machines.Find(index + 1);
            if (nextMachine == null) return;

            if (nextMachine.machineState == MachineState.READY)
            {
                nextMachine.SetState(MachineState.REPOSITION);
            }
        }


        public void ResetReadyMachine(Machine currentMachine, int index)
        {
            //������ ���� �� ��
            //�׷��� ���� ���� ������ ���̸� ���� �� �� �־����.
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
            if (!isPrevious) // ������
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







































//õ���� ��
//õ���� ����(����) �� ��
//�ڵ�ȭ �ý��ۿ��� ����Ǵ� �����縦
//�߰� ���� �� ���������ν� ���� ��ġ�ִ� ����� ���׷��̵� �� �Ǹ��ϴ� ����

//�ӽ� �ϳ��� 1���� ���׸��� ���� �� �ִ�. (���� �Һ� �ȵǸ� ����)
//���� ��Ÿ���� �����Ѵ�.

// ������Ʈ Ǯ��
// ������ ���� ����
// ������ ���ͷ���