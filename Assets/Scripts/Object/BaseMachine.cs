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
        [HideInInspector]
        public bool m_isStop = false; // 생산머신 특징 - 정지 함

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
        }
        protected override void IdleSequence()
        {
            Debug.Log(this.name + " : 이동중");
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
            sequence.AppendInterval(this.Model.m_Data.Machine_Create_Speed * 0.5f).OnComplete(() => {

                this.CreateBaseMaterial(ThousandLinesManager.Instance.m_MaterialObject);
                this.SetState(MachineState.MOVE);
            });
        }

        protected override void MoveSequence()
        {
            base.MoveSequence();
            this.m_MaterialObject.transform.DOLocalPath(this.m_Pos, this.Model.m_Data.Machine_Speed * 0.5f).OnComplete(() =>
            {
                this.SetState(MachineState.WAIT);
            });
        }

        protected override void WaitSequence()
        {
            base.WaitSequence();
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