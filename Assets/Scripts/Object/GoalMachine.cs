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
            if (ThousandLinesManager.Instance.m_InMachines.Count > this.SettingIndex)
            {
                // 널체크 부터- 코드정리 전

                // 순서
                // 1. 내 뒤에 Null 있는지 확인 후 압축 (리스트 정리)
                for (int i = this.SettingIndex + 1; i < ThousandLinesManager.Instance.m_InMachines.Count; i++)
                {
                    if (ThousandLinesManager.Instance.m_InMachines[i] == null)
                    {
                        ThousandLinesManager.Instance.m_InMachines.Remove(ThousandLinesManager.Instance.m_InMachines[i]);
                        this.SettingIndex = i -1;
                    }
                }
                // 2. 널이 아닌경우 인데 추가된 사항이 있는경우
                //    - 해당 항목을 In 으로 대체시키고 자신은 뒤로 빠진다.
                // 할만한데?

                //라인 매니저 327 번째 줄 꼭 참고

                // 리스트 인덱스 재정의 하고 돌려야 한다.
                // 본인 제거 해야한다.
                bool isReset = false;
                for (int i = this.SettingIndex + 1; i < ThousandLinesManager.Instance.m_InMachines.Count; i++)
                {
                    if (ThousandLinesManager.Instance.m_InMachines[i] != null)
                    {
                        ThousandLinesManager.Instance.m_InMachines.Swap(i, this.SettingIndex);
                        ThousandLinesManager.Instance.m_InMachines[i - 1].SettingIndex = this.SettingIndex;
                        this.SettingIndex = ThousandLinesManager.Instance.m_InMachines.Count -1;
                        isReset = true;
                        ThousandLinesManager.Instance.m_InMachines[i -1].SetState(MachineState.IN);
                    }
                }

                if (isReset)
                {
                    ThousandLinesManager.Instance.SetSortingGroup(this.gameObject, this.SettingIndex);
                    this.SetState(MachineState.REPOSITION);
                    isReset = false;
                }

                //Debug.LogError(this.SettingIndex);
            }
            //머신 리스트를 재정리 한다.

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