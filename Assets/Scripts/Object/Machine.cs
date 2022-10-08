using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ThousandLines
{
    public class Machine : MonoBehaviour
    {
        public int Index;
        public  MachineState machineState = MachineState.NULL;
        protected List<SpriteRenderer> m_SpriteRenderers;
        public MaterialObject m_MaterialObject;

        [SerializeField]
        protected Animator m_Animator;

        [SerializeField]
        protected List<Transform> m_tr;
        protected Vector3[] m_Pos;

        [HideInInspector]
        protected Dictionary<MachineState, Action> m_Actions = new Dictionary<MachineState, Action>();

        protected virtual void Awake()
        {
            this.InitializeSprites();
            this.InitializePos();
            this.InitializeAnimator();
        }
        public virtual void Show()
        {
            this.SetupSequence();
        }

        #region Initialize

        private void InitializeSprites()
        {
            this.m_SpriteRenderers = new List<SpriteRenderer>();
            this.m_SpriteRenderers = SpriteExtensions.GetSpriteList(this.gameObject);
            SpriteExtensions.HideSpriteObject(this.m_SpriteRenderers);
        }

        private void InitializePos()
        {
            this.m_Pos = GetPoints();
        }
        private Vector3[] GetPoints()
        {
            return this.m_tr.ConvertAll(c => c.position).ToArray();
        }

        private void InitializeAnimator()
        {
            if (this.GetComponent<Animator>() != null) 
                this.m_Animator = this.GetComponent<Animator>();
        }

        #endregion

        #region SetUpSequences

        protected virtual void SetupSequence()
        {
            this.m_Actions.Add(MachineState.INITIALIZE, this.InitializeSequence);
            this.m_Actions.Add(MachineState.READY, this.ReadySequence);
            this.m_Actions.Add(MachineState.PLAY, this.PlaySequence);
            this.m_Actions.Add(MachineState.MOVE, this.MoveSequence);
            this.m_Actions.Add(MachineState.WAIT, this.WaitSequence);
            this.SetState(MachineState.INITIALIZE);
        }
        protected virtual void InitializeSequence()
        {
            Debug.Log(this.name + " : 초기화 시작");
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, Color.white, 0.5f));
            sequence.AppendInterval(0.5f);
        }
        protected virtual void MoveSequence()
        {
            Debug.Log(this.name + " : 이동중");
        }

        protected virtual void WaitSequence()
        {
            Debug.Log(this.name + " : 대기중");
        }
        protected virtual void ReadySequence()
        {
            Debug.Log(this.name + " : 준비완료");
        }
        protected virtual void PlaySequence()
        {
            Debug.Log(this.name + " : 작동");
        }

        #endregion

        public void SetState(MachineState state)
        {
            if (this.machineState == state)
                return;

            this.machineState = state;
            var action = this.m_Actions.Find(state);
            if (action != null)
                action.Invoke();
        }
    }
}