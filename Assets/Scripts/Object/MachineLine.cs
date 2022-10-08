using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : Machine
    {
        [SerializeField]
        private MachineLineModel Model;

        protected override void Awake()
        {
            base.Awake();

        }
        public override void Show()
        {
            base.Show();
        }

        public void SetMachine(MachineLineData machineLineData)
        {
            var model = new MachineLineModel(machineLineData);
            this.SetMachine(model);
        }

        private void SetMachine(MachineLineModel machineModel)
        {
            this.Model = machineModel;
        }

        protected override void InitializeSequence()
        {
            base.InitializeSequence();
        }

        protected override void ReadySequence()
        {
            base.ReadySequence();
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();
        }


        protected override void MoveSequence()
        {
            base.MoveSequence();
        }
        protected override void WaitSequence()
        {
            base.WaitSequence();
        }
    }
}