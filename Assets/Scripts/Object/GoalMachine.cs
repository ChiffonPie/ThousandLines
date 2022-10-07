using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class GoalMachine : MonoBehaviour
    {
        [HideInInspector]
        public GoalMachineState goalMachineState = GoalMachineState.NULL;
        private List<SpriteRenderer> m_SpriteRenderers;
        private Dictionary<GoalMachineState, Action> m_Actions = new Dictionary<GoalMachineState, Action>();

        public void Show()
        {
            this.InitializeGoalMachine();
        }
        private void InitializeGoalMachine()
        {
            this.InitializeSprites();
            this.SetupSequence();
        }
        private void InitializeSprites()
        {
            this.m_SpriteRenderers = new List<SpriteRenderer>();
            this.m_SpriteRenderers = SpriteExtensions.GetSpriteList(this.gameObject);
            SpriteExtensions.HideSpriteObject(this.m_SpriteRenderers);
        }
        private void SetupSequence()
        {
            this.m_Actions.Add(GoalMachineState.INITIALIZE, this.InitializeSequence);
            this.m_Actions.Add(GoalMachineState.READY, this.ReadySequence);
            this.m_Actions.Add(GoalMachineState.GOAL, this.GoalSequence);
            this.SetState(GoalMachineState.INITIALIZE);
        }
        private void SetState(GoalMachineState state)
        {
            if (this.goalMachineState == state)
                return;

            this.goalMachineState = state;
            var action = this.m_Actions.Find(state);
            if (action != null)
                action.Invoke();
        }

        private void InitializeSequence()
        {
            Debug.Log(this.name + " : 초기화중");
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(this.m_SpriteRenderers, Color.white, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                Debug.Log(this.name + " : 초기화 완료");
                if (this.goalMachineState == GoalMachineState.INITIALIZE)
                    this.SetState(GoalMachineState.READY);
            });
        }
        private void ReadySequence()
        {
            Debug.Log(this.name + " : 준비완료");
        }

        private void GoalSequence()
        {
            Debug.Log(this.name + " : 준비완료");
        }
    }
}