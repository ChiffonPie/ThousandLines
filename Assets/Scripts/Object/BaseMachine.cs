using DG.Tweening;
using UnityEngine;
using ThousandLines_Data;

namespace ThousandLines
{
    public class BaseMachine : Machine
    {
        [HideInInspector]
        public bool m_isStop = false;
        public BaseMachineModel Model;

        protected override void Awake()
        {
            base.Awake();
            this.InitializeBaseMachine();
        }

        public override void Show()
        {
            base.Show();
        }

        #region Initialized And Set Sequence

        private void InitializeBaseMachine()
        {
            var data = AssetDataManager.GetData<BaseMachineData>(1);
            var model = new BaseMachineModel(data);
            this.SetMachine(model);
        }

        private void SetMachine(BaseMachineModel machineModel)
        {
            this.Model = machineModel;
        }

        #endregion

        #region Sequences

        protected override void InitializeSequence()
        {
            base.InitializeSequence();
            //초기화 시간 지정 - 0.5f

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.Join(this.m_MovingBoard.defaultM.DOColor(Color.white, 0.5f));
            sequence.AppendInterval(0.5f).OnComplete(() =>
            {
                this.SetState(MachineState.READY);
            });
        }

        protected override void ReadySequence()
        {
            base.ReadySequence();
            if (!m_isStop)
                this.SetState(MachineState.STOP);
            else
                this.SetState(MachineState.PLAY);
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();

            var sequence = DOTween.Sequence();
            this.SetBoardSpeed = this.Model.m_Data.Machine_Create_Speed * 0.9f;
            sequence.AppendInterval(this.Model.m_Data.Machine_Create_Speed * 0.5f).OnComplete(() => {

                this.CreateBaseMaterial(ThousandLinesManager.Instance.m_MaterialObject);
                this.SetState(MachineState.MOVE);
            });
        }

        protected override void MoveSequence()
        {
            base.MoveSequence();

            this.m_MaterialObject.transform.DOLocalPath(this.m_Pos, this.Model.m_Data.Machine_Speed * 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                this.SetState(MachineState.WAIT);
            });
        }

        protected override void WaitSequence()
        {
            base.WaitSequence();
            this.SetBoardSpeed = 0;
            ThousandLinesManager.Instance.MachineReceive(this);
        }
        protected override void InSequence()
        {
            base.InSequence();
        }
        protected override void OutSequence()
        {
            base.OutSequence();
        }

        protected override void StopSequence()
        {
            base.StopSequence();
            this.m_stateSr.color = Color.red;
        }

        #endregion

        #region Others

        private void CreateBaseMaterial(MaterialObject materialObject)
        {
            MaterialObject createMaterial = Instantiate(materialObject, this.m_tr[0]);
            createMaterial.name = materialObject.name;
            createMaterial.transform.localPosition = this.m_Pos[0];
            this.m_MaterialObject = createMaterial;
        }

        #endregion
    }
}