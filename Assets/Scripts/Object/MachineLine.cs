using DG.Tweening;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MachineLine : Machine
    {
        private Transform? materialTr;
        private MachineLineModel Model;
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
            this.animationTime = this.GetAnimationTime();
        }

        public void SetMachine(MachineLineData machineLineData)
        {
            var model = new MachineLineModel(machineLineData);
            this.Model = model;
        }

        protected override void InitializeSequence()
        {
            base.InitializeSequence();

            //애니메이션 시간을 가져와서 총 타임에 맞춰야 함.
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
                this.SetState(MachineState.MOVE);
            });
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();
            var sequence = DOTween.Sequence();

            // 2. 가공 대기상태 (애니메이션 시작)
            this.StartAnimation(1);
            sequence.AppendInterval(this.animationTime).OnComplete(() =>
            {
                this.isComplete = true;
                this.SetState(MachineState.MOVE);
            });
        }

        protected override void MoveSequence()
        {
            base.MoveSequence();
            var sequence = DOTween.Sequence();

            if (!isComplete)
            {
                // 1. 재료 중앙 이동
                sequence.Append(this.m_MaterialObject.transform.DOMove(this.m_Pos[0], 1)).OnComplete(() =>
                {
                    this.SetMaterialParent(materialTr);
                    this.SetState(MachineState.PLAY);
                });
            }
            else
            {
                // 3. 가공 완료 후 다음으로 이동
                this.SetMaterialParent(ThousandLinesManager.Instance.transform);
                sequence.Append(this.m_MaterialObject.transform.DOMove(this.m_Pos[1], 1)).OnComplete(() =>
                {
                    this.SetState(MachineState.WAIT);
                });
            }
        }
        protected override void WaitSequence()
        {
            base.WaitSequence();
            this.isComplete = false;

            //작업이 완료되어 대기중임을 매니저에게 전달
            ThousandLinesManager.Instance.BaseMachineNext(this, () =>
            {
                this.SetState(MachineState.READY);
            });
        }
    }
}