using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : MonoBehaviour
    {
        [SerializeField]
        private MachineLineModel Model;

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        private void SetMachine(MachineLineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}