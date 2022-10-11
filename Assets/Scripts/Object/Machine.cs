using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UniRx;
using UnityEngine;

namespace ThousandLines
{
    public class Machine : MonoBehaviour
    {
        [Header("[ Machine ]")]
        public int Index;
        public float m_Distace; // 입력해도 되고, 데이터로 로드 해도됨
        public  MachineState machineState = MachineState.NULL;
        public MaterialObject m_MaterialObject;

        [SerializeField]
        protected SpriteRenderer m_stateSr;

        protected List<SpriteRenderer> m_SpriteRenderers;
        protected SpriteRenderer m_buttonSpriteRenderer;
        protected Animator m_Animator;

        [SerializeField]
        protected List<Transform> m_tr;
        protected Vector3[] m_Pos;
        private bool m_Reposition = false; // 해제로 인한 이동 예약

        [HideInInspector]
        public bool m_isReserved = false; //해제 및 설치 예약

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
            if (this.m_stateSr != null) this.m_stateSr.color = Color.red;

            //버튼 스프라이트 적용
            for (int i = 0; i < this.m_SpriteRenderers.Count; i++)
            {
                if (this.m_SpriteRenderers[i].name =="MachineButton")
                {
                    this.m_buttonSpriteRenderer = this.m_SpriteRenderers[i];
                    this.m_buttonSpriteRenderer.color = Color.green;
                }
            }
        }

        private void SetupPos()
        {
            this.m_Pos = GetPoints();
        }
        private Vector3[] GetPoints()
        {
            return this.m_tr.ConvertAll(c => c.localPosition).ToArray();
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
            this.m_Actions.Add(MachineState.READY,      this.ReadySequence);
            this.m_Actions.Add(MachineState.PLAY,       this.PlaySequence);
            this.m_Actions.Add(MachineState.MOVE,       this.MoveSequence);
            this.m_Actions.Add(MachineState.WAIT,       this.WaitSequence);
            this.m_Actions.Add(MachineState.IN,         this.InSequence);
            this.m_Actions.Add(MachineState.OUT,        this.OutSequence);
            this.m_Actions.Add(MachineState.STOP,       this.StopSequence);
            this.m_Actions.Add(MachineState.REPOSITION, this.RepositionSequence);
            this.SetState(MachineState.INITIALIZE);
        }
        protected virtual void InitializeSequence()
        {
            //초기화 시간 지정 - 0.5f
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.AppendInterval(0.5f).OnComplete(() => 
            {
                this.SetState(MachineState.READY);
            });
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

        protected virtual void InSequence()
        {
            Debug.Log(this.name + " : 설치");
        }

        protected virtual void OutSequence()
        {
            Debug.Log(this.name + " : 해제");

            int index = this.Index;
            ThousandLinesManager.Instance.MachineListRemove(this);

            //만약 다음 친구가 READY 면 Call 해야함
            //나는 확정 OUT
            ThousandLinesManager.Instance.ResetReadyMachine(this, index);

            Vector2 hidePos = this.transform.position;
            hidePos += new Vector2(0.78f, 1f);

            //떠날 때 이전 머신이 Ready 상태일 경우 불러준다.

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 1f, false));
            sequence.Join(this.transform.DOMove(hidePos, 1f));
        }

        protected virtual void RepositionSequence()
        {
            // 해제 머신의 다음 머신들은 모든 생산을 완료 한 뒤 이동 하여야 한다.
            Debug.Log(this.name + " : 포지션 재정리");

            int index = this.Index;
            ThousandLinesManager.Instance.MachineListSet(this);
            ThousandLinesManager.Instance.ResetRepositionMachine(this, index);

            //용규야 단순하게 생각해
            //이동 시작하면 다음 애 그냥 끌고오면 되는거야

            Sequence sequence = DOTween.Sequence();
            Vector2 movePos = ThousandLinesManager.Instance.GetMachineLinePos(this);

            sequence.Append(this.transform.DOMove(movePos, 1f)).OnComplete(() =>
            {
                this.SetState(MachineState.READY);
            });
        }

        protected virtual void StopSequence()
        {

        }

        #endregion

        #region SetState

        public void SetState(MachineState state)
        {
            if (this.machineState == state)
                return;

            this.machineState = state;
            this.SetStateColor(this.machineState);
            var action = this.m_Actions.Find(state);
            if (action != null)
                action.Invoke();
        }

        private void SetStateColor(MachineState state)
        {
            if (this.m_stateSr == null) return;
            switch (machineState)
            {
                case MachineState.INITIALIZE: return;
                case MachineState.READY:      this.m_stateSr.color = Color.cyan;   return;
                case MachineState.PLAY:       this.m_stateSr.color = Color.green;  return;
                case MachineState.OUT:        this.m_stateSr.color = Color.red;    return;
                case MachineState.REPOSITION: this.m_stateSr.color = Color.yellow; return;
            }
            this.m_stateSr.color = new Color(1, 0.5f, 0);
        }

        public void SetInAndOut(bool isReserved)
        {
            if (this.machineState == MachineState.OUT) return;
            this.m_isReserved = isReserved;
            if (!this.m_isReserved)
                this.m_buttonSpriteRenderer.color = Color.green;
            else
                this.m_buttonSpriteRenderer.color = Color.red;

            if (this.machineState == MachineState.READY)
                this.SetState(MachineState.OUT);
        }

        public void SetStopMachine(Machine baseMachine)
        {
            //Stop 은 오로지 베이스 머신만 가능
            var baseMachineC = baseMachine.GetComponent<BaseMachine>();
            baseMachineC.m_isStop = !baseMachineC.m_isStop;
            if (!baseMachineC.m_isStop)
                this.m_buttonSpriteRenderer.color = Color.green;
            else
                this.m_buttonSpriteRenderer.color = Color.red;

            if (this.machineState == MachineState.STOP)
            {
                this.SetState(MachineState.READY);
            }
        }

        #endregion

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

        public void SetAnimationSpeed(float speed)
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