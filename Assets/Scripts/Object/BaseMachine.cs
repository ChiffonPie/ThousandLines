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
        private Transform[] m_tr;
        private Vector3[] m_Pos;

        public void Show()
        {
            this.InitializeBaseMachine();
        }

        #region Initialized And Set Sequence

        private void InitializeBaseMachine()
        {
            this.InitializeSprites();
            this.InitializedPos();
            var data = AssetDataManager.GetData<BaseMachineData>(1);
            var model = new BaseMachineModel(data);
            this.SetMachine(model);
            this.SetState(BaseMachineState.INITIALIZE);
        }

        private void InitializedPos()
        {
            this.m_Pos = new Vector3[this.m_tr.Length];
            for (int i = 0; i < this.m_Pos.Length; i++)
            {
                this.m_Pos[i] = this.m_tr[i].position;
            }
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
        private void InitializeSequence()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, Color.white, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                Debug.Log(this.name + " : 초기화 완료");
                this.SetState(BaseMachineState.CREATE);
            });
        }

        private void CreateSequence()
        {
            var sequence = DOTween.Sequence();
            Debug.LogError(this.Model.m_Data.Machine_Create_Speed);

            sequence.AppendInterval(this.Model.m_Data.Machine_Create_Speed);
            //Join 해서 로딩게이지 연출 추가

            sequence.AppendCallback(() =>
            {
                Debug.Log(this.name + " : 생성");
                this.CreateBaseMaterial(ThousandLinesManager.Instance.m_MaterialObject);
                this.SetState(BaseMachineState.MOVE);
            });
        }

        private void MoveSequence()
        {
            Debug.Log(this.m_MaterialObject + " : 이동중");
            this.m_MaterialObject.transform.DOPath(this.m_Pos, this.Model.m_Data.Machine_Speed, PathType.CatmullRom)
                .OnComplete(() =>
                {
                    this.SetState(BaseMachineState.WAIT);
                });
        }

        private void WaitSequence()
        {
            Debug.Log(this.name + " : 대기중");
        }

        private void ReadySequence()
        {
            Debug.Log(this.name + " : 준비완료");
        }

        #endregion


        #region Others

        private void CreateBaseMaterial(MaterialObject materialObject)
        {
            MaterialObject createMaterial = Instantiate(materialObject, this.transform);
            createMaterial.name = materialObject.name;
            materialObject.transform.position = this.m_Pos[0];
            this.m_MaterialObject = materialObject;
        }

        #endregion
    }
}