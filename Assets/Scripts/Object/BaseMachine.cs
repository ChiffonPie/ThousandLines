using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ThousandLines_Data;
using UniRx;

namespace ThousandLines
{
    public class BaseMachine : MonoBehaviour
    {
        public BaseMachineModel Model;
        public MaterialObject m_MaterialObject;

        [HideInInspector]
        public BaseMachineState baseMachineState = BaseMachineState.NULL;
        private List<SpriteRenderer> m_SpriteRenderers;

        private Dictionary<BaseMachineState, Action> m_Actions = new Dictionary<BaseMachineState, Action>();
        [SerializeField]
        private Transform m_createPos;
        public void Show()
        {
            this.InitializeBaseMachine();
        }


        #region Initialized And Set Sequence

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
            this.m_Actions.Add(BaseMachineState.INITIALIZE, this.InitializeSequence);
            this.m_Actions.Add(BaseMachineState.READY, this.ReadySequence);
            this.m_Actions.Add(BaseMachineState.CREATE, this.CreateSequence);
            this.m_Actions.Add(BaseMachineState.MOVE, this.MoveSequence);
            this.m_Actions.Add(BaseMachineState.WAIT, this.WaitSequence);
            this.SetState(BaseMachineState.INITIALIZE);
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

        #endregion

        #region Sequences

        private void CreateSequence()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(this.Model.m_Data.Machine_Create_Speed);
            sequence.AppendCallback(() =>
            {
                Debug.LogError("생성");
                this.SetState(BaseMachineState.MOVE);
            });
        }

        private void InitializeSequence()
        {
            Debug.LogError("초기화중");
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, Color.white, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                this.CreateBaseMaterial(ThousandLinesManager.Instance.m_MaterialObject);
                this.SetState(BaseMachineState.MOVE);
            });
        }

        private void MoveSequence()
        {
            Debug.LogError("이동중");
        }

        private void WaitSequence()
        {
            Debug.LogError("대기중");
        }

        private void ReadySequence()
        {
            Debug.LogError("준비완료");
        }

        #endregion


        #region Others

        private void CreateBaseMaterial(MaterialObject materialObject)
        {
            MaterialObject createMaterial = Instantiate(materialObject, this.transform);
            materialObject.name = materialObject.name;
            materialObject.transform.position = m_createPos.transform.position;
        }

        #endregion
    }
}