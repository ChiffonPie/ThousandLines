using UnityEngine;

namespace ThousandLines
{
    public class Machine : MonoBehaviour
    {
        [SerializeField]
        private MachineModel Model;

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        private void SetMachine(MachineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}