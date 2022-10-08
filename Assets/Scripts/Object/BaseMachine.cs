using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using ThousandLines_Data;
using UniRx;

namespace ThousandLines
{
    public class BaseMachine : Machine
    {
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
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                this.SetState(MachineState.READY);
            });
        }
        protected override void ReadySequence()
        {
            base.ReadySequence();
            this.SetState(MachineState.PLAY);
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(this.Model.m_Data.Machine_Create_Speed).OnComplete(() => {

                this.CreateBaseMaterial(ThousandLinesManager.Instance.m_MaterialObject);
                this.SetState(MachineState.MOVE);
            });
            //Join 해서 로딩게이지 연출 추가
        }

        protected override void MoveSequence()
        {
            base.MoveSequence();
            var sequence = DOTween.Sequence();
            this.m_MaterialObject.transform.DOPath(this.m_Pos, this.Model.m_Data.Machine_Speed).OnComplete(() =>
            {
                this.SetState(MachineState.WAIT);
            });
        }

        protected override void WaitSequence()
        {
            base.WaitSequence();
            //작업이 완료되어 대기중임을 매니저에게 전달
            ThousandLinesManager.Instance.BaseMachineNext(this ,() => 
            {
                this.SetState(MachineState.READY);
            });
        }



        #endregion

        #region Others



        private void CreateBaseMaterial(MaterialObject materialObject)
        {
            MaterialObject createMaterial = Instantiate(materialObject, ThousandLinesManager.Instance.transform);
            createMaterial.name = materialObject.name;
            createMaterial.transform.position = this.m_Pos[0];
            this.m_MaterialObject = createMaterial;
        }

        #endregion
    }
}