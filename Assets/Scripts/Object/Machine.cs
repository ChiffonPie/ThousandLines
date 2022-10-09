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
        [Header("[ Machine ]")]
        public int Index;
        public  MachineState machineState = MachineState.NULL;
        public MaterialObject m_MaterialObject;

        protected List<SpriteRenderer> m_SpriteRenderers;
        protected Animator m_Animator;

        [SerializeField]
        protected List<Transform> m_tr;

        [SerializeField]
        protected Vector3[] m_Pos;

        [HideInInspector]
        protected Dictionary<MachineState, Action> m_Actions = new Dictionary<MachineState, Action>();

        protected virtual void Awake()
        {
            this.InitializeSprites();
            this.InitializeAnimator();
        }
        public virtual void Show()
        {
            this.SetupSequence();
            this.SetupPos();
        }

        #region Initialize

        private void InitializeSprites()
        {
            this.m_SpriteRenderers = new List<SpriteRenderer>();
            this.m_SpriteRenderers = SpriteExtensions.GetSpriteList(this.gameObject);
            SpriteExtensions.HideSpriteObject(this.m_SpriteRenderers);
        }

        private void SetupPos()
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
            {
                this.m_Animator = this.GetComponent<Animator>();
                this.m_Animator.speed = 0;
            }
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

        #region Animation

        protected float GetAnimationTime()
        {
            return this.m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        protected virtual void SetMaterialParent(Transform parent = null)
        {
            if (parent != null)
            {
                this.m_MaterialObject.transform.SetParent(parent);
            }
        }

        public void SetAnimation(float speed)
        {
            this.m_Animator.Rebind();
            this.m_Animator.speed = speed;
        }
        #endregion

        #region Prosseing

        protected void PressScale(MaterialObject materialObject)
        {
            if (materialObject == null) return;

            //압축에 대한 연출
            materialObject.transform.localScale = 
                new Vector2(materialObject.transform.localScale.x * 1.5f, materialObject.transform.localScale.y * 0.8f);
        }

        protected void AddSprite(MaterialObject materialObject, string spriteName)
        {
            //용접에 대한 연출
            if (materialObject == null) return;
            var sprite = Resources.Load<Sprite>($"{"Sprites/PNG/Prosseing/" + spriteName}");

            SpriteRenderer spriteRenderer = null;
            for (int i = 0; i < materialObject.transform.childCount; i++)
            {
                if (materialObject.transform.GetChild(i).name == spriteName)
                {
                    materialObject.transform.GetChild(i).transform.localPosition = Vector3.zero;
                    materialObject.transform.GetChild(i).transform.localScale = Vector3.one;

                    spriteRenderer = materialObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.sortingOrder = materialObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
                    return;
                }
            }

            //없으면 처리
            GameObject weldingObject = new GameObject();
            weldingObject.transform.SetParent(materialObject.transform);
            weldingObject.transform.localPosition = Vector3.zero;
            weldingObject.transform.localScale = Vector3.one;
            weldingObject.name = spriteName;

            spriteRenderer = weldingObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = materialObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        protected void ChangeColor()
        {
            //담금질에 대한 연출
        }

        #endregion
    }
}