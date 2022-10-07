using System.Collections.Generic;
using UnityEngine;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        //��ü���� ���� �ý��� �� �������� �����Ѵ�.
        public static ThousandLinesManager Instance { get; private set; }

        public BaseMachine m_BaseMachine;
        public GoalMachine m_GoalMachine;
        public MachineLine m_MachineLinePrefab;
        public MaterialObject m_MaterialObject;

        [SerializeField]
        private List<MachineLine> m_MachineLines = new List<MachineLine>();
        private List<MaterialObject> m_MaterialObjects;

        private void Awake()
        {
            Instance = this;
            this.m_MaterialObjects = new List<MaterialObject>();
            this.m_MachineLines = new List<MachineLine>();
        }

        #region Initialize
        public void Initiaize()
        {
            this.InitializeBaseMachine();
            this.InitializeMachineLine();
            this.InitializeGoalMachine();
        }

        private void InitializeBaseMachine()
        {
            BaseMachine baseMachine = Instantiate(this.m_BaseMachine, this.transform);
            baseMachine.name = this.m_BaseMachine.name;
            baseMachine.transform.position = Vector3.zero;

            this.m_BaseMachine = baseMachine;
            this.m_BaseMachine.Show();
        }

        private void InitializeMachineLine()
        {

        }

        private void InitializeGoalMachine()
        {
            GoalMachine goalMachine = Instantiate(this.m_GoalMachine, this.transform);
            goalMachine.name = this.m_GoalMachine.name;
            this.m_GoalMachine = goalMachine;
            this.m_GoalMachine.Show();
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