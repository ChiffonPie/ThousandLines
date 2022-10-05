using UnityEngine;

namespace ThousandLines
{
    public class Machine : MonoBehaviour
    {
        [SerializeField]
        private MachineModel Model;

        //처음 생성시 만들어지는 기본 데이터 메테리얼 오브젝트
        private void SetMachine(MachineModel machineModel)
        {
            this.Model = machineModel;
        }
    }
}