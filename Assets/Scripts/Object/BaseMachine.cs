using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ThousandLines_Data;

namespace ThousandLines
{
    public class BaseMachine : MonoBehaviour
    {
        public BaseMachineModel Model;
        public BaseMachineState baseMachineState = BaseMachineState.NULL;
        private List<SpriteRenderer> m_SpriteRenderers;
        
        private Dictionary<BaseMachineState, Action> m_Actions = new Dictionary<BaseMachineState, Action>();
        [SerializeField]
        private Transform m_createPos;
        public void Show()
        {
            this.InitializeBaseMachine();
        }
        private void InitializeBaseMachine()
        {
            this.InitializeSprites();
            var data = AssetDataManager.GetData<BaseMachineData>(1);
            var model = new BaseMachineModel(data);
            this.SetMachine(model);
            this.SetState(BaseMachineState.INITIALIZE);
        }
        private void InitializeSprites()
        {
            this.m_SpriteRenderers = new List<SpriteRenderer>();
            this.m_SpriteRenderers = SpriteExtensions.GetSpriteList(this.gameObject);
            SpriteExtensions.HideSpriteObject(this.m_SpriteRenderers);
        }
        private void SetMachine(BaseMachineModel machineModel)
        {
            this.Model = machineModel;
            this.SetupSequence();
        }
        private void SetupSequence()
        {
            this.m_Actions.Add(BaseMachineState.INITIALIZE, this.Initialize);
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
            sequence.AppendCallback(() =>
            {
                Debug.LogError("����");

                //if (this.baseMachineState == BaseMachineState.CREATE)
                //    this.SetState(BaseMachineState.MOVE);
            });
        }

        private void Initialize()
        {
            Debug.LogError("�ʱ�ȭ��");
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, Color.white, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                Debug.LogError("�ʱ�ȭ �Ϸ�");
                if (this.baseMachineState == BaseMachineState.INITIALIZE)
                    this.SetState(BaseMachineState.READY);
            });
        }

        private void Move()
        {
            Debug.LogError("�̵���");
        }

        private void Wait()
        {
            Debug.LogError("�����");
        }

        private void Ready()
        {
            Debug.LogError("�غ�Ϸ�");
        }


    }
}