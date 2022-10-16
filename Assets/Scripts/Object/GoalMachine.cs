using DG.Tweening;
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
            if (isPullForward) return;

            this.ClearInMachineList();
            if (isResetPosition) return;

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
            ThousandLinesManager.Instance.Money = this.CalculationValue();
            this.m_MaterialObject.DestroyMaterialObject();
            this.m_MaterialObject = null;
        }

        private double CalculationValue()
        {
            double value = this.m_MaterialObject.Value;
            //리스트의 오더 인덱스로 추가값을 계산

            for (int i = 0; i < this.m_MaterialObject.processingList.Count; i++)
            {
                for (int j = 0; j < ThousandLinesManager.Instance.m_Machines.Count; j++)
                {
                    if (ThousandLinesManager.Instance.m_Machines[j].Model.m_Data.OrderIndex == this.m_MaterialObject.processingList[i])
                    {
                        var getValue = ThousandLinesManager.Instance.m_Machines[j].Model.m_Data.Line_Prosseing_Value;

                        switch (ThousandLinesManager.Instance.m_Machines[i].processingType)
                        {
                            case ProcessingType.NULL:                                break;
                            case ProcessingType.PRESS:    value += getValue;         break;
                            case ProcessingType.WELDING:  value *= getValue;         break;
                            case ProcessingType.SOAK:     value += getValue;         break;
                        }
                    }
                }
            }
            return value;
        }

        #region Others

        //한칸씩 앞으로 전진
        private bool isPullForward
        {
            get
            {
                //앞으로 밀착
                if (ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex - 1] == null)
                {
                    ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex - 1] = this;
                    this.CurrentIndex--;
                    ThousandLinesManager.Instance.m_InMachines.Remove(ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex]);
                    this.SetState(MachineState.REPOSITION);
                    return true;
                }
                return false;
            }
        }

        private void ClearInMachineList()
        {
            //리스트 클리어 처리
            for (int i = this.CurrentIndex + 1; i < ThousandLinesManager.Instance.m_InMachines.Count;)
            {
                if (ThousandLinesManager.Instance.m_InMachines[i] == null)
                    ThousandLinesManager.Instance.m_InMachines.Remove(ThousandLinesManager.Instance.m_InMachines[i]);
                else
                    i++;
            }
        }

        private bool isResetPosition
        {
                //포지션 재정리 여부
            get 
            {
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
                    return true;
                }

                return false;
            }
        }

        #endregion
    }
}