using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : MonoBehaviour
    {
        [SerializeField]
        private MachineLineModel Model;
        public MachineLineState baseMachineState = MachineLineState.NULL;

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        private void SetMachine(MachineLineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}