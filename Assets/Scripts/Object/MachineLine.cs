using DG.Tweening;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : Machine
    {
        [Header("[ MachineLine ]")]
        [SerializeField]
        private Transform prosseingTr;
        public MachineLineModel Model;
        public ProcessingType processingType = ProcessingType.NULL;
        private float animationTime;
        private bool isComplete = false;

        protected override void Awake()
        {
            base.Awake();
            this.isComplete = false;
        }
        public override void Show()
        {
            base.Show();
        }

        #region Initialized And Set Sequence

        public void SetMachine(MachineLineData machineLineData)
        {
            var model = new MachineLineModel(machineLineData);
            this.processingType = EnumExtension.ProsseingStringToEnum(model.m_Data.Line_Prosseing);
            this.animationTime = this.GetAnimationTime();
            this.id = model.m_Data.Id;
            this.Model = model;
            this.m_Distace = Model.m_Data.Line_Distance;
        }

        protected override void InitializeSequence()
        {
            base.InitializeSequence();

            if (this.Model.m_Data.Line_Setting_Index == 0)
            {
                this.SetState(MachineState.OUT);
                return;
            }

            //초기화 시간 지정 - 0.5f
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.m_MovingBoard.defaultM.DOColor(Color.white, 0.5f));
            sequence.Join(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.AppendInterval(0.5f).OnComplete(() =>
            {
                this.SetState(MachineState.READY);
            });
        }

        #endregion

        #region Sequence

        protected override void ReadySequence()
        {
            base.ReadySequence();
            if (this != ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex])
            {
                this.SetState(MachineState.REPOSITION);
                return;
            }

            if (this.m_isReserved)
                this.SetState(MachineState.OUT);
            else
                ThousandLinesManager.Instance.MachineSend(this);
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();
            var sequence = DOTween.Sequence();

            // 2. 가공 시작 (애니메이션 시작)
            this.SetAnimationSpeed(1);
            sequence.AppendInterval(this.animationTime).OnComplete(() =>
            {
                this.isComplete = true;
                this.SetAnimationSpeed(0);
                this.SetState(MachineState.MOVE);
            });
        }

        protected override void MoveSequence()
        {
            base.MoveSequence();
            var sequence = DOTween.Sequence();
            this.SetBoardSpeed = this.Model.m_Data.Line_Speed * 0.5f;
            if (!isComplete)
            {
                // 1. 재료 중앙 이동
                sequence.Append(this.m_MaterialObject.transform.DOLocalMove(this.m_Pos[0], this.Model.m_Data.Line_Speed * 0.5f).SetEase(Ease.Linear));
                sequence.Append(this.m_MaterialObject.transform.DOLocalMove(this.m_Pos[1], this.Model.m_Data.Line_Speed * 0.5f).SetEase(Ease.Linear)).OnComplete(() =>
                {
                     this.SetBoardSpeed = 0;
                     this.m_MaterialObject.processingList.Add(this.Model.m_Data.OrderIndex);
                     this.SetMaterialParent(prosseingTr);
                     this.SetState(MachineState.PLAY);
                });
            }
            else
            {
                // 3. 가공 완료 후 다음으로 이동
                this.SetMaterialParent(this.transform);
                sequence.Append(this.m_MaterialObject.transform.DOLocalMove(this.m_Pos[2], this.Model.m_Data.Line_Speed * 0.5f).SetEase(Ease.Linear)).OnComplete(() =>
                {
                    this.SetState(MachineState.WAIT);
                });
            }
        }

        protected override void WaitSequence()
        {
            base.WaitSequence();
            this.SetBoardSpeed = 0;
            this.isComplete = false;
            ThousandLinesManager.Instance.MachineReceive(this);
        }

        protected override void InSequence()
        {
            base.InSequence();

            this.gameObject.SetActive(true);
            Vector2 hidePos = ThousandLinesManager.Instance.GetMachineLinePos(this); // 좌표 계산식 참고
            Vector2 startPos = new Vector2(0.78f, 1f);

            this.transform.position = new Vector2(startPos.x + hidePos.x, startPos.y);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 1f, true));
            sequence.Join(this.transform.DOMove(hidePos, 1f)).OnComplete(() =>
            {
                this.m_isReserved = false;
                this.SetState(MachineState.READY);
            });
        }

        protected override void OutSequence()
        {
            if (this.CurrentIndex == 0) return;
            base.OutSequence();
        }

        protected override void RepositionSequence()
        {
            base.RepositionSequence();
        }

        #endregion

        #region Others

        public void ProsseingMatertial()
        {
            switch (processingType)
            {
                case ProcessingType.NULL: Debug.LogError("처리 과정이 정의되지 않았습니다."); break;
                case ProcessingType.PRESS: this.PressScale(this.m_MaterialObject); break;
                case ProcessingType.WELDING: this.AddSprite(this.m_MaterialObject, this.Model.m_Data.Line_Prosseing); break;
                case ProcessingType.SOAK: this.ChangeColor(this.m_MaterialObject, Color.green); break;
            }
        }

        #endregion
    }
}