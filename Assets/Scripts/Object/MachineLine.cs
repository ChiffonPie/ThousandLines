using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : MonoBehaviour
    {
        [SerializeField]
        private MachineLineModel Model;
        public MachineLineState baseMachineState = MachineLineState.NULL;

        //처음 생성시 만들어지는 기본 데이터 메테리얼 오브젝트
        private void SetMachine(MachineLineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}