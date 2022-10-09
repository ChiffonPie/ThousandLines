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
            ThousandLinesManager.Instance.MachinePrevious(this, () =>
            {
                this.SetState(MachineState.PLAY);
            });

            //���׸����� �������� ���� ������ ��� ����Ѵ�.
            //�غ� ���¿����� ���� �ܰ��� Wait�� �޾Ƽ� ó���� �� �ֵ��� �Ѵ�.
        }

        //�� ������ ��� vector �� ���۰��� ���� ���͸��� ������Ʈ ��ǥ�� �޾Ƽ� �߰��Ѵ�.

        protected override void PlaySequence()
        {
            base.PlaySequence();

        }
       
        protected override void MoveSequence()
        {
            base.MoveSequence();
            var sequence = DOTween.Sequence();
            this.m_GoalPos[0] = this.m_MaterialObject.transform.position;
            sequence.Append(this.m_MaterialObject.transform.DOPath(this.m_Pos, 1)).OnComplete(() =>
            {
                this.SetMoney();
                this.SetState(MachineState.READY);
            });
        }

        private void SetMoney()
        {
            ThousandLinesManager.Instance.Money = this.m_MaterialObject.Value;
            Destroy(this.m_MaterialObject.gameObject);
            this.m_MaterialObject = null;

            //������Ʈ Ǯ������ �����ϱ� �ʿ�

        }

    }
}