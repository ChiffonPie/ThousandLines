using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace ThousandLines
{
    public class BaseMachine : MonoBehaviour
    {
        public BaseMachineModel Model;
        public BaseMachineState baseMachineState = BaseMachineState.NULL;
        
        private Dictionary<BaseMachineState, Action> m_Actions = new Dictionary<BaseMachineState, Action>();
        [SerializeField]
        private Transform m_createPos;

        //베이스 재료 생성 및 이동 처리
        private void SetMachine(BaseMachineModel machineModel)
        {
            this.Model = machineModel;
            this.SetupSequence();
        }
        private void SetupSequence()
        {
            this.m_Actions.Add(BaseMachineState.READY, this.Ready);
            this.m_Actions.Add(BaseMachineState.CREATE, this.Create);
            this.m_Actions.Add(BaseMachineState.MOVE, this.Move);
            this.m_Actions.Add(BaseMachineState.WAIT, this.Wait);

            this.SetState(BaseMachineState.READY);
        }

        private void SetState(BaseMachineState state)
        {
            if (this.baseMachineState == state)
                return;

            this.baseMachineState = state;
            var action = this.m_Actions.Find(state);
            if (action != null)
                action.Invoke();
        }

        private void Create()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1);
            sequence.AppendCallback(() =>
            {
                if (this.baseMachineState == BaseMachineState.CREATE)
                    this.SetState(BaseMachineState.MOVE);
            });
        }

        private void Move()
        {

        }

        private void Wait()
        {

        }

        private void Ready()
        {

        }
    }
}