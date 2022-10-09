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

            //�ִϸ��̼� �ð��� �����ͼ� �� Ÿ�ӿ� ����� ��.
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

            // 2. ���� ������ (�ִϸ��̼� ����)
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
                // 1. ��� �߾� �̵�
                sequence.Append(this.m_MaterialObject.transform.DOMove(this.m_Pos[0], 1)).OnComplete(() =>
                {
                    this.SetMaterialParent(materialTr);
                    this.SetState(MachineState.PLAY);
                });
            }
            else
            {
                // 3. ���� �Ϸ� �� �������� �̵�
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

            //�۾��� �Ϸ�Ǿ� ��������� �Ŵ������� ����
            ThousandLinesManager.Instance.BaseMachineNext(this, () =>
            {
                this.SetState(MachineState.READY);
            });
        }
    }
}