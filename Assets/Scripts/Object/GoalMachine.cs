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
            //�ʱ�ȭ �ð� ���� - 0.5f
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

            //����Ʈ üũ�ϰ� �����Ѵ�.
            //���� �ӽ��� �����ϴ��� ���ο� ���� ���� ���ġ �Ѵ�.

            //���� �߰��Ǿ� ��������� Ȯ��
            if (ThousandLinesManager.Instance.m_InMachines.Count > this.SettingIndex)
            {
                // ��üũ ����- �ڵ����� ��

                // ����
                // 1. �� �ڿ� Null �ִ��� Ȯ�� �� ����
                for (int i = this.SettingIndex + 1; i < ThousandLinesManager.Instance.m_InMachines.Count; i++)
                {
                    if (ThousandLinesManager.Instance.m_InMachines[i] == null)
                    {
                        ThousandLinesManager.Instance.m_InMachines.Remove(ThousandLinesManager.Instance.m_InMachines[i]);
                        this.SettingIndex = i;
                    }
                }
                // 2. ���� �ƴѰ�� �ε� �߰��� ������ �ִ°��
                //    - �ش� �׸��� In ���� ��ü��Ű�� �ڽ��� �ڷ� ������.
                // �Ҹ��ѵ�?

                // ����Ʈ �ε��� ������ �ϰ� ������ �Ѵ�.


                for (int i = this.SettingIndex + 1; i < ThousandLinesManager.Instance.m_InMachines.Count; i++)
                {
                    if (ThousandLinesManager.Instance.m_InMachines[i] != null)
                    {
                        ThousandLinesManager.Instance.m_InMachines[i].SetState(MachineState.IN);
                        this.SetState(MachineState.REPOSITION);
                    }
                }

            }
            //�ӽ� ����Ʈ�� ������ �Ѵ�.

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