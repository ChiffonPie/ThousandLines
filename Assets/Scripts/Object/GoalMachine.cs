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
        private Vector3[] m_GoalPos;

        protected override void Awake()
        {
            base.Awake();
            this.GoalVector();
        }

        private void GoalVector()
        {
            this.m_GoalPos = new Vector3[m_Pos.Length + 1];
            for (int i = 1; i < m_GoalPos.Length; i++)
            {
                this.m_GoalPos[i] = this.m_Pos[i - 1];
            }
        }

        public override void Show()
        {
            base.Show();
            this.GoalVector();
        }

        protected override void InitializeSequence()
        {
            base.InitializeSequence();
            this.SetState(MachineState.READY);
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
            this.m_GoalPos[0] = this.m_MaterialObject.transform.position;
            sequence.Append(this.m_MaterialObject.transform.DOLocalPath(this.m_Pos, 1)).OnComplete(() =>
            {
                this.SetMoney();
                this.SetState(MachineState.READY);
            });
        }
        protected override void WaitSequence()
        {
            base.WaitSequence();
            ThousandLinesManager.Instance.MachineReceive(this);
        }

        private void SetMoney()
        {
            ThousandLinesManager.Instance.Money = this.m_MaterialObject.Value;
            Destroy(this.m_MaterialObject.gameObject);
            this.m_MaterialObject = null;
        }
    }
}