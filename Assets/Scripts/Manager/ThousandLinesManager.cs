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
        //��ü���� ���� �ý��� �� �������� �����Ѵ�.

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

            //���̺� Ȱ��ȭ �� �ӽſ� ���� �����Ѵ�.

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
            //���̺� �̸��� �´� ������ ������ �ε�
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

        // �ý��� ����
        // �� ��ü�� ���� ��ü�� Wait ������ ��� ���� �۵� �������� �ߵ��Ѵ�.
        public void BaseMachineNext(Machine currentMachine, Action onCompleted)
        {
            //���� �ӽ��� ���������� �ǳ׹��� �� �ִ��� Ȯ��
            var nextMachine = m_Machine.Find(currentMachine.Index + 1);
            if (nextMachine == null) return;
            if (nextMachine.machineState != MachineState.READY) return;

            nextMachine.m_MaterialObject = currentMachine.m_MaterialObject;
            currentMachine.m_MaterialObject = null;

            nextMachine.SetState(MachineState.MOVE);

            onCompleted?.Invoke();
        }

        //���� ��ü�� �Ϸ����, ���� �Ϸᰡ �Ǹ� ȣ���
        public void MachinePrevious(Machine currentMachine, Action onCompleted)
        {
            //���� �ӽ��� ���������� �ǳ׹��� �� �ִ��� Ȯ��
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







































//õ���� ��
//õ���� ����(����) �� ��
//�ڵ�ȭ �ý��ۿ��� ����Ǵ� �����縦
//�߰� ���� �� ���������ν� ���� ��ġ�ִ� ����� ���׷��̵� �� �Ǹ��ϴ� ����

//�ӽ� �ϳ��� 1���� ���׸��� ���� �� �ִ�. (���� �Һ� �ȵǸ� ����)
//���� ��Ÿ���� �����Ѵ�.

// ������Ʈ Ǯ��
// ������ ���� ����
// ������ ���ͷ���