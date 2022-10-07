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

        //처음 생성시 만들어지는 기본 데이터 메테리얼 오브젝트
        private void SetMachine(MachineLineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}