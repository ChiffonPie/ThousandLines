using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThousandLines
{
    public class Machine : MonoBehaviour
    {
        [Header("[ Machine ]")]
        public string id;
        public int m_Index;
        public float m_Distace; // 입력하거나, 데이터 로드 (데이터 로드 우선)
        public MachineState machineState = MachineState.NULL;
        public MaterialObject m_MaterialObject;

        [SerializeField]
        protected SpriteRenderer m_stateSr;
        protected List<SpriteRenderer> m_SpriteRenderers;
        
        [SerializeField]
        protected SpriteRenderer m_buttonSpriteRenderer;
        protected Animator m_Animator;

        [SerializeField]
        protected List<Transform> m_tr;
        protected Vector3[] m_Pos;

        [SerializeField]
        protected MovingBoard m_MovingBoard;

        [HideInInspector]
        public bool m_isReserved = false; //해제 및 설치 예약

        [HideInInspector]
        protected Dictionary<MachineState, Action> m_Actions = new Dictionary<MachineState, Action>();

        public int CurrentIndex
        {
            get { return m_Index; }
            set
            {
                ThousandLinesManager.Instance.SetSortingGroup(this.gameObject, value);
                this.m_Index = value;
            }
        }
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
            this.m_Actions.Add(MachineState.READY, this.ReadySequence);
            this.m_Actions.Add(MachineState.PLAY, this.PlaySequence);
            this.m_Actions.Add(MachineState.MOVE, this.MoveSequence);
            this.m_Actions.Add(MachineState.WAIT, this.WaitSequence);
            this.m_Actions.Add(MachineState.IN, this.InSequence);
            this.m_Actions.Add(MachineState.OUT, this.OutSequence);
            this.m_Actions.Add(MachineState.STOP, this.StopSequence);
            this.m_Actions.Add(MachineState.REPOSITION, this.RepositionSequence);
            this.SetState(MachineState.INITIALIZE);
        }
        protected virtual void InitializeSequence()
        {
            Debug.Log(this.name + " : Initialize");
        }
        protected virtual void MoveSequence()
        {
            Debug.Log(this.name + " : Move");
        }

        protected virtual void WaitSequence()
        {
            Debug.Log(this.name + " : Wait");
        }
        protected virtual void ReadySequence()
        {
            Debug.Log(this.name + " : Ready");
        }
        protected virtual void PlaySequence()
        {
            Debug.Log(this.name + " : Play");
        }

        protected virtual void InSequence()
        {
            Debug.Log(this.name + " : In");
            Sequence sequence = DOTween.Sequence();
            sequence.Join(this.m_MovingBoard.defaultM.DOColor(Color.white, 1f));
        }
        protected virtual void OutSequence()
        {
            Debug.Log(this.name + " : Out");

            int index = this.CurrentIndex;
            ThousandLinesManager.Instance.MachineListRemove(this);
            ThousandLinesManager.Instance.ResetReadyMachine(this, index);

            Vector2 hidePos = this.transform.position;
            hidePos += new Vector2(0.78f, 1f);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(this.m_SpriteRenderers, 1f, false));
            sequence.Join(this.m_MovingBoard.defaultM.DOColor(Color.clear, 1f));

            sequence.Join(this.transform.DOMove(hidePos, 1f)).OnComplete(() =>
            {
                this.m_buttonSpriteRenderer.color = Color.green;
                ThousandLinesUIManager.Instance.SetInteractableButton(this.id);

                this.gameObject.SetActive(false);
            });
        }

        protected virtual void RepositionSequence()
        {
            Debug.Log(this.name + " : Reposition");
            Sequence sequence = DOTween.Sequence();

            if (this != ThousandLinesManager.Instance.m_InMachines[this.CurrentIndex])
            {
                for (int i = 0; i < ThousandLinesManager.Instance.m_InMachines.Count; i++)
                {
                    if (ThousandLinesManager.Instance.m_InMachines[i] == this)
                    {
                        this.CurrentIndex = i;
                        break;
                    }
                }

                Vector2 movePos1 = ThousandLinesManager.Instance.GetMachineLinePos(this);
                sequence.Append(this.transform.DOMove(movePos1, 0.5f).SetEase(Ease.Linear)).OnComplete(() =>
                {
                    this.SetState(MachineState.READY);
                });
                return;
            }

            int index = this.CurrentIndex;
            ThousandLinesManager.Instance.MachineListSet(this);
            ThousandLinesManager.Instance.ResetRepositionMachine(index);

            Vector2 movePos = ThousandLinesManager.Instance.GetMachineLinePos(this);

            sequence.Append(this.transform.DOMove(movePos, 0.5f).SetEase(Ease.Linear)).OnComplete(() =>
            {
                this.SetState(MachineState.READY);
            });
        }

        protected virtual void StopSequence()
        {
            Debug.Log(this.name + " : 정지");
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
            switch (state)
            {
                case MachineState.INITIALIZE: return;
                case MachineState.READY: this.m_stateSr.color = Color.cyan; return;
                case MachineState.PLAY: this.m_stateSr.color = Color.green; return;
                case MachineState.OUT: this.m_stateSr.color = Color.red; return;
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
                this.SetState(MachineState.READY);
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
                    materialObject.transform.GetChild(i).gameObject.SetActive(true);
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
            weldingObject.transform.gameObject.SetActive(true);
        }

        protected void ChangeColor(MaterialObject materialObject, Color color)
        {
            //담금질에 대한 연출
            materialObject.m_SpriteRenderer.color = color;
        }

        #endregion

        #region Other

        protected float SetBoardSpeed
        {
            set
            {
                if (this.m_MovingBoard != null)
                    this.m_MovingBoard.speed = value;
            }
        }
        #endregion
    }
}