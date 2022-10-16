using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThousandLines_Data;
using UnityEngine.Rendering;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        //��ü���� ���� �ý��� �� �������� �����Ѵ�.
        public static ThousandLinesManager Instance { get; private set; }

        [Header(" [ Objects ] ")]
        public BaseMachine m_BaseMachine;
        public GoalMachine m_GoalMachine;
        public MaterialObject m_MaterialObject;

        public double m_money;
        public List<Machine> m_InMachines = new List<Machine>();
        public List<Machine> m_OutMachines = new List<Machine>(); //������� �ӽ�

        [SerializeField]
        public List<MaterialObject> m_MaterialObjects;

        private void Awake()
        {
            Instance = this;
            this.m_MaterialObjects = new List<MaterialObject>();
        }

        #region Initialize

        public void Initiaize()
        {
            var machineLineDatas = AssetDataManager.GetDatas<MachineLineData>();
            this.StartCoroutine(InitializeCoroutine(machineLineDatas));
        }

        private IEnumerator InitializeCoroutine(List<MachineLineData> machineLineDatas)
        {
            //������� �ʱ�ȭ
            this.InitializeMaterialObjects();

            //���� �ӽ� �ʱ�ȭ
            this.InitializeBaseMachine();
            yield return new WaitForSeconds(0.5f);

            //���� �ӽ� - ���̺� Ȱ��ȭ �� �ӽſ� ���� �����Ѵ�.
            for (int i = 0; i < machineLineDatas.Count; i++)
            {
                if (machineLineDatas[i].Line_Setting_Index != 0)
                    this.m_InMachines.Add(this.InitializeMachineLine(machineLineDatas[i]));
                else
                {
                    var offMachineLine = this.InitializeMachineLine(machineLineDatas[i]);
                    offMachineLine.gameObject.SetActive(false);
                    this.m_OutMachines.Add(offMachineLine);
                }
            }

            //��ġ���� �͵� �ʱ�ȭ
            for (int i = 1; i < m_InMachines.Count; i++)
            {
                this.m_InMachines[i].Show();
                yield return new WaitForSeconds(0.5f);
            }

            //��ġ���� ���� ����� �ʱ�ȭ
            for (int i = 0; i < m_OutMachines.Count; i++)
            {
                this.m_OutMachines[i].Show();
            }

            // ���� �ӽ� �ʱ�ȭ
            this.InitializeGoalMachine();
            yield return new WaitForSeconds(1f);

            //����Ʈ ����
            List<MachineLine> allMachineLine = new List<MachineLine>();
            allMachineLine.AddRange(this.FindMachineLine(this.m_InMachines));
            allMachineLine.AddRange(this.FindMachineLine(this.m_OutMachines));

            // UI �ʱ�ȭ
            ThousandLinesUIManager.Instance.Initialize(allMachineLine);
            ThousandLinesUIManager.Instance.SetAcitveGameUI(true);

            //���� �Ϸ� �� ��ġ Ȱ��ȭ
            InputManager.Instance.IsLock = false;
        }

        private List<MachineLine> FindMachineLine(List<Machine> machines)
        {
            List<MachineLine> machineLines = new List<MachineLine>();
            for (int i = 0; i < machines.Count; i++)
            {
                if (machines[i].GetComponent<MachineLine>() != null)
                {
                    machineLines.Add(machines[i].GetComponent<MachineLine>());
                }
            }
            return machineLines;
        }

        private void InitializeMaterialObjects()
        {
            //������ ��� ����Ʈ ����
            this.m_MaterialObjects = new List<MaterialObject>();
            var data = AssetDataManager.GetDatas<MaterialObjectData>();
            for (int i = 0; i < data.Count; i++)
            {
                MaterialObject materialObject = Instantiate(this.m_MaterialObject, this.transform);
                materialObject.SetMaterialObject(data[i]);
                this.m_MaterialObjects.Add(materialObject);
            }

            this.m_MaterialObject = this.m_MaterialObjects[0];
            ThousandLinesUIManager.Instance.InitializeMaterialToggles(this.m_MaterialObjects);
        }

        private void InitializeBaseMachine()
        {
            //����ӽ� �ʱ�ȭ
            BaseMachine baseMachine = Instantiate(this.m_BaseMachine, this.transform);
            baseMachine.name = this.m_BaseMachine.name;
            this.m_BaseMachine = baseMachine;

            this.SetSortingGroup(this.m_BaseMachine.gameObject, 0);
            this.m_BaseMachine.CurrentIndex = this.m_InMachines.Count;
            this.m_InMachines.Add(this.m_BaseMachine);

            this.m_BaseMachine.Show();
        }

        private MachineLine InitializeMachineLine(MachineLineData machineLineData)
        {
            //���̺� �̸��� �´� ������ ������ �ε�
            MachineLine machineLine = Instantiate(Resources.Load<MachineLine>($"{"Prefabs/MachineLine/" + machineLineData.Id}"), this.transform);
            machineLine.name = machineLineData.Id;
            machineLine.SetMachine(machineLineData);
            machineLine.m_Distace = machineLineData.Line_Distance;
            machineLine.CurrentIndex = machineLine.Model.m_Data.Line_Setting_Index;

            this.SetSortingGroup(machineLine.gameObject, machineLine.CurrentIndex);
            machineLine.transform.position = GetMachineLinePos(machineLine);
            return machineLine;
        }

        private void InitializeGoalMachine()
        {
            GoalMachine goalMachine = Instantiate(this.m_GoalMachine, this.transform);
            goalMachine.name = this.m_GoalMachine.name;
            this.m_GoalMachine = goalMachine;
            this.m_GoalMachine.CurrentIndex = this.m_InMachines.Count;
            this.SetSortingGroup(this.m_GoalMachine.gameObject, this.m_GoalMachine.CurrentIndex);

            this.m_GoalMachine.transform.position = GetMachineLinePos(this.m_GoalMachine);
            this.m_InMachines.Add(this.m_GoalMachine);

            this.m_GoalMachine.Show();
        }

        #endregion

        #region MachineState

        // Receive - Wait ���¿��� (�ڱ��� ���� �� ���) �� ȣ��
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

            this.SendMaterialObject(nextMachine, currentMachine);

            nextMachine.SetState(MachineState.MOVE);
            currentMachine.SetState(MachineState.READY);
        }

        // Send - ������ �Ϸ� �� ���� ��󿡰� ȣ��
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
            this.SendMaterialObject(currentMachine, previousMachine);

            //�߿� - �ϰŸ� ������ �����Ѵ�
            if (previousMachine.m_MaterialObject == null)
                currentMachine.SetState(MachineState.MOVE);

            previousMachine.SetState(MachineState.READY);
        }

        //���� �ӽ��� ��ġ �� ���� ȣ��
        public void ResetRepositionMachine(int index)
        {
            var nextMachine = this.m_InMachines.Find(index + 1);
            if (nextMachine == null) return;

            if (nextMachine.machineState == MachineState.READY)
            {
                nextMachine.SetState(MachineState.REPOSITION);
            }
        }

        // �����Ǵ� ����� �����ӽ��� �̵��� ���� ȣ��
        public void ResetReadyMachine(Machine currentMachine, int index)
        {
            //������ ���� �� ��
            //�׷��� ���� ���� ������ ���̸� ���� �� �� �־����.
            var nextMachine = this.m_InMachines.Find(index + 1);
            if (nextMachine == null) return;

            if ((currentMachine.machineState == MachineState.REPOSITION ||
                currentMachine.machineState == MachineState.OUT) &&
                nextMachine.machineState == MachineState.READY)
            {
                nextMachine.SetState(MachineState.REPOSITION);
            }
        }

        #endregion

        #region Others

        // �۾��� ��Ḧ �Ѱ���
        private void SendMaterialObject(Machine setMachine, Machine getMachine)
        {
            setMachine.m_MaterialObject = getMachine.m_MaterialObject;
            setMachine.m_MaterialObject.transform.SetParent(setMachine.transform);
            getMachine.m_MaterialObject = null;
        }


        // �� ����, Ȥ�� ���� �ӽ��� ã�� ����
        private Machine FindMachine(Machine currentMachine, bool isPrevious = false)
        {
            if (!isPrevious) // ���� ���
            {
                var findMachine = this.m_InMachines.Find(currentMachine.CurrentIndex - 1);
                if (findMachine != null) return findMachine;
            }
            else // ���� ���
            {
                var findMachine = this.m_InMachines.Find(currentMachine.CurrentIndex + 1);
                if (findMachine != null) return findMachine;
            }
            return null;
        }

        // ������ ����� ����Ʈ���� ����
        public void MachineListRemove(Machine machine)
        {
            this.m_OutMachines.Add(machine);
            this.m_InMachines[machine.CurrentIndex] = null;
        }

        // ����Ʈ ����
        public void MachineListSet(Machine machine)
        {
            if (machine == this.m_GoalMachine) return;
            for (int i = 0; i < this.m_InMachines.Count; i++)
            {
                if (this.m_InMachines[i] == null)
                {
                    this.m_InMachines[i] = machine;
                    this.m_InMachines[machine.CurrentIndex] = null;
                    machine.CurrentIndex = i;
                    return;
                }
            }
        }

        // UI ���� ȣ��� �ӽ� ��ġ
        public void SettingMachine(string machineId)
        {
            var machine = this.GetMachine(this.m_OutMachines, machineId);
            this.m_OutMachines.Remove(machine);
            this.m_InMachines.Add(machine);

            if (this.m_GoalMachine.machineState == MachineState.READY)
            {
                for (int i = 1; i <= this.m_GoalMachine.CurrentIndex; i++)
                {
                    if (this.m_InMachines[i] == null)
                        return;
                }
                if (this.m_InMachines[this.m_GoalMachine.CurrentIndex - 1].machineState == MachineState.READY ||
                    this.m_InMachines[this.m_GoalMachine.CurrentIndex - 1].machineState == MachineState.STOP)
                {
                    this.m_GoalMachine.SetState(MachineState.REPOSITION);
                }
            }
        }

        // Ư�� ����Ʈ�� �ִ� �ӽ� ã�� ( ID )
        public Machine GetMachine(List<Machine> machines ,string id)
        {
            for (int i = 0; i < machines.Count; i++)
            {
                if (machines[i].id == id)
                    return machines[i];
            }
            return null;
        }
        
        // ��ġ �̵��� ���� ��ǥ ���ϱ�
        public Vector2 GetMachineLinePos(Machine machine)
        {
            float xPos = machine.m_Distace;
            for (int i = 0; i < machine.CurrentIndex; i++)
            {
                if (this.m_InMachines[i] != null && this.m_InMachines[i] != m_GoalMachine)
                {
                    xPos += this.m_InMachines[i].m_Distace;
                }
            }
            return new Vector2((xPos), 0);
        }

        // ������ ���� Sorting Order ����
        public void SetSortingGroup(GameObject gameObject, int index)
        {
            gameObject.GetComponent<SortingGroup>().sortingOrder = index;
        }

        public double Money
        {
            set
            {
                this.m_money += value;
                ThousandLinesUIManager.Instance.m_moneyText.text = m_money.ToString();

                //�Ӵ� ���Žø��� ������ �� �ִ� �׸��� Ȱ��ȭ �� �ش�.
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