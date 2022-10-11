using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class GoalMachine : Machine
    {
        public float Line_Distance = 2.7f;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Show()
        {
            base.Show();
        }

        #region Sequences

        protected override void InitializeSequence()
        {
            base.InitializeSequence();
            this.SetState(MachineState.READY);
        }
        protected override void IdleSequence()
        {
            base.IdleSequence();
        }

        protected override void ReadySequence()
        {
            base.ReadySequence();
            ThousandLinesManager.Instance.MachineSend(this);
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();
        }
       
        protected override void MoveSequence()
        {
            base.MoveSequence();
            if (this.m_MaterialObject == null)
            {
                this.SetState(MachineState.PLAY);
                return;
            }

            var sequence = DOTween.Sequence();
            this.SetMaterialParent(this.transform);
            sequence.Append(this.m_MaterialObject.transform.DOLocalPath(this.m_Pos, 1)).OnComplete(() =>
            {
                this.SetMoney();
                this.SetState(MachineState.READY);
            });
        }
        protected override void WaitSequence()
        {
            base.WaitSequence();
            ThousandLinesManager.Instance.MachineSend(this);
        }
        protected override void RepositionSequence()
        {
            base.RepositionSequence();
        }

        #endregion

        private void SetMoney()
        {
            ThousandLinesManager.Instance.Money = this.m_MaterialObject.Value;
            Destroy(this.m_MaterialObject.gameObject);
            this.m_MaterialObject = null;
        }
    }
}