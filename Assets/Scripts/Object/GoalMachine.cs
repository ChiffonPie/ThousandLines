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
        [SerializeField]
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
        }

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
            ThousandLinesManager.Instance.MachinePrevious(this, () =>
            {
                this.SetState(MachineState.PLAY);
            });

            //메테리얼을 보유하지 않은 상태인 경우 대기한다.
            //준비 상태에서는 이전 단계의 Wait를 받아서 처리할 수 있도록 한다.
        }

        //골 지점의 경우 vector 의 시작값을 현재 메터리얼 오브젝트 좌표를 받아서 추가한다.

        protected override void PlaySequence()
        {
            base.PlaySequence();
            this.m_GoalPos[0] = this.m_MaterialObject.transform.position;
            this.m_MaterialObject.transform.DOPath(this.m_GoalPos, 1).OnComplete(() =>
            {
                this.SetMoney();
                this.SetState(MachineState.READY);
            });
        }
       
        protected override void MoveSequence()
        {
            base.MoveSequence();
        }

        private void SetMoney()
        {
            ThousandLinesManager.Instance.Money = this.m_MaterialObject.Value;
            Destroy(this.m_MaterialObject.gameObject);
            this.m_MaterialObject = null;

            //오브젝트 풀링으로 제어하기 필요

        }

    }
}