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
        public float m_Distace; // �Է��ص� �ǰ�, �����ͷ� �ε� �ص���
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
        private bool m_Reposition = false; // ������ ���� �̵� ����

        [HideInInspector]
        public bool m_isReserved = false; //���� �� ��ġ ����

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

            //��ư ��������Ʈ ����
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
            //�ʱ�ȭ �ð� ���� - 0.5f
            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.AppendInterval(0.5f).OnComplete(() => 
            {
                this.SetState(MachineState.READY);
            });
        }
        protected virtual void MoveSequence()
        {
            Debug.Log(this.name + " : �̵���");
        }

        protected virtual void WaitSequence()
        {
            Debug.Log(this.name + " : �����");
        }
        protected virtual void ReadySequence()
        {
            Debug.Log(this.name + " : �غ�Ϸ�");
        }
        protected virtual void PlaySequence()
        {
            Debug.Log(this.name + " : �۵�");
        }

        protected virtual void InSequence()
        {
            Debug.Log(this.name + " : ��ġ");
        }

        protected virtual void OutSequence()
        {
            Debug.Log(this.name + " : ����");

            int index = this.Index;
            ThousandLinesManager.Instance.MachineListRemove(this);

            //���� ���� ģ���� READY �� Call �ؾ���
            //���� Ȯ�� OUT
            ThousandLinesManager.Instance.ResetReadyMachine(this, index);

            Vector2 hidePos = this.transform.position;
            hidePos += new Vector2(0.78f, 1f);

            //���� �� ���� �ӽ��� Ready ������ ��� �ҷ��ش�.

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 1f, false));
            sequence.Join(this.transform.DOMove(hidePos, 1f));
        }

        protected virtual void RepositionSequence()
        {
            // ���� �ӽ��� ���� �ӽŵ��� ��� ������ �Ϸ� �� �� �̵� �Ͽ��� �Ѵ�.
            Debug.Log(this.name + " : ������ ������");

            int index = this.Index;
            ThousandLinesManager.Instance.MachineListSet(this);
            ThousandLinesManager.Instance.ResetRepositionMachine(this, index);

            //��Ծ� �ܼ��ϰ� ������
            //�̵� �����ϸ� ���� �� �׳� ������� �Ǵ°ž�

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
            //Stop �� ������ ���̽� �ӽŸ� ����
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

            //���࿡ ���� ����
            materialObject.transform.localScale = 
                new Vector2(materialObject.transform.localScale.x * 1.5f, materialObject.transform.localScale.y * 0.8f);
        }

        protected void AddSprite(MaterialObject materialObject, string spriteName)
        {
            //������ ���� ����
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

            //������ ó��
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
            //������� ���� ����
        }

        #endregion
    }
}