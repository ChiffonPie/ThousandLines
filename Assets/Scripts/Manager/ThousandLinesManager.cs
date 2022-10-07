using System.Collections.Generic;
using UnityEngine;

namespace ThousandLines
{
    public class ThousandLinesManager : MonoBehaviour
    {
        //��ü���� ���� �ý��� �� �������� �����Ѵ�.
        public static ThousandLinesManager Instance { get; private set; }

        [SerializeField]
        private BaseMachine m_BaseMachine;
        [SerializeField]
        private GoalMachine m_GoalMachine;
        [SerializeField]
        private MachineLine m_MachineLinePrefab;

        private Dictionary<int, MachineLine> m_MachineLines = new Dictionary<int, MachineLine>();
        private List<MaterialObject> m_MaterialObjects;

        private void Awake()
        {
            Instance = this;
        }
        public void Initiaize()
        {
            this.InitializeBaseMachine();
            this.InitializeMachineLine();
        }

        private void InitializeBaseMachine()
        {
            BaseMachine baseMachine = Instantiate(this.m_BaseMachine,this.transform);
            baseMachine.transform.position = Vector3.zero;
            baseMachine.Show();
        }

        private void InitializeMachineLine()
        {

        }
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