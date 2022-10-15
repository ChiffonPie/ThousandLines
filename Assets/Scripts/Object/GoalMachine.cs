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
            //초기화 시간 지정 - 0.5f
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.AppendInterval(0.5f).OnComplete(() =>
            {
                this.SetState(MachineState.READY);
            });
        }

        protected override void ReadySequence()
        {
            base.ReadySequence();

            //누가 추가되어 대기중인지 확인
            //최신 리스트 상태에 따라서 위치조정을 해야함.

            //앞으로 밀착

            if (ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex - 1] == null)
            {
                ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex - 1] = this;
                this.CurrentIndex--;
                ThousandLinesManager.Instance.m_InMachines.Remove(ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex]);
                this.SetState(MachineState.REPOSITION);
                return;
            }
            //리스트 클리어 처리

            for (int i = this.CurrentIndex + 1; i < ThousandLinesManager.Instance.m_InMachines.Count;)
            {
                if (ThousandLinesManager.Instance.m_InMachines[i] == null)
                {
                    ThousandLinesManager.Instance.m_InMachines.Remove(ThousandLinesManager.Instance.m_InMachines[i]);
                }
                else
                {
                    i++;
                }
            }

            //포지션 재정리
            if (this.CurrentIndex < ThousandLinesManager.Instance.m_InMachines.Count - 1)
            {
                for (int i = 1; i < ThousandLinesManager.Instance.m_InMachines.Count; i++)
                {
                    if (ThousandLinesManager.Instance.m_InMachines[i].machineState == MachineState.OUT)
                    {
                        ThousandLinesManager.Instance.m_InMachines[i].CurrentIndex = i;
                        ThousandLinesManager.Instance.m_InMachines[i].SetState(MachineState.IN);
                    }
                }

                ThousandLinesManager.Instance.m_InMachines.Remove(this);
                ThousandLinesManager.Instance.m_InMachines.Add(this);
                this.CurrentIndex = ThousandLinesManager.Instance.m_InMachines.Count - 1;

                this.SetState(MachineState.REPOSITION);
                return;
            }

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
            sequence.Append(this.m_MaterialObject.transform.DOLocalPath(this.m_Pos, 1).SetEase(Ease.Linear)).OnComplete(() =>
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