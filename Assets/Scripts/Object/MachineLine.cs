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
        private MachineAbility machineAbility = MachineAbility.NULL;
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
            this.machineAbility = EnumExtension.ProsseingStringToEnum(model.m_Data.Line_Prosseing);
            this.animationTime = this.GetAnimationTime();
            this.Model = model;
            this.m_Distace = Model.m_Data.Line_Distance;
        }

        protected override void InitializeSequence()
        {
            base.InitializeSequence();
            //�ʱ�ȭ �ð� ���� - 0.5f

            //��ġ�� ���
            if (this.Model.m_Data.Line_isActive == 0)
            {
                this.SetState(MachineState.OUT);
                return;
            }

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.AppendInterval(0.5f).OnComplete(() =>
            {
                //��ġ�� ���
                this.SetState(MachineState.READY);
            });
        }

        #endregion

        #region Sequence

        protected override void ReadySequence()
        {
            base.ReadySequence();
            if (this.m_isReserved)
            {
                this.SetState(MachineState.OUT);
            }
            else
            {
                ThousandLinesManager.Instance.MachineSend(this);
            }
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();
            var sequence = DOTween.Sequence();

            // 3. ���� ������ (�ִϸ��̼� ����)
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

            if (!isComplete)
            {
                // 2. ��� �߾� �̵�
                sequence.Append(this.m_MaterialObject.transform.DOLocalMove(this.m_Pos[0], this.Model.m_Data.Line_Speed * 0.5f)).OnComplete(() =>
                {
                    this.SetMaterialParent(prosseingTr);
                    this.SetState(MachineState.PLAY);
                });
            }
            else
            {
                // 3. ���� �Ϸ� �� �������� �̵�
                this.SetMaterialParent(this.transform);
                sequence.Append(this.m_MaterialObject.transform.DOLocalMove(this.m_Pos[1], this.Model.m_Data.Line_Speed * 0.5f)).OnComplete(() =>
                {
                    this.SetState(MachineState.WAIT);
                });
            }
        }

        protected override void WaitSequence()
        {
            base.WaitSequence();
            this.isComplete = false;
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

        protected override void RepositionSequence()
        {
            base.RepositionSequence();
        }

        #endregion

        #region Others

        public void ProsseingMatertial()
        {
            switch (machineAbility)
            {
                case MachineAbility.NULL:    Debug.LogError("ó�� ������ ���ǵ��� �ʾҽ��ϴ�.");  break;
                case MachineAbility.PRESS:   this.PressScale(this.m_MaterialObject);    break;
                case MachineAbility.WELDING: this.AddSprite (this.m_MaterialObject, this.Model.m_Data.Line_Prosseing);    break;
                case MachineAbility.SOAK:    this.ChangeColor();   break;
            }
        }

        #endregion
    }
}