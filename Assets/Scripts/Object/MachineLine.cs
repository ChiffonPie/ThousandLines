using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : MonoBehaviour
    {
        [SerializeField]
        private MachineLineModel Model;
        public MachineLineState baseMachineState = MachineLineState.NULL;
        private int m_Index;

        public int Index
        {
            get { return this.m_Index; }
            set { this.m_Index = value; }
        }

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        private void SetMachine(MachineLineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}