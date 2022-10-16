using DG.Tweening;
using UnityEngine;
using ThousandLines_Data;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace ThousandLines
{
    public class BaseMachine : Machine
    {
        public ObjectPool<MaterialObject> materialObjectPool;

        [HideInInspector]
        public bool m_isStop = false;
        public BaseMachineModel Model;

        protected override void Awake()
        {
            base.Awake();
            this.InitializeBaseMachine();
            this.InitializeObjectPool();
        }

        public override void Show()
        {
            base.Show();
        }

        #region Initialized And Set Sequence

        private void InitializeBaseMachine()
        {
            var data = AssetDataManager.GetData<BaseMachineData>(1);
            var model = new BaseMachineModel(data);
            this.SetMachine(model);
        }

        private void SetMachine(BaseMachineModel machineModel)
        {
            this.Model = machineModel;
        }

        #endregion

        #region Sequences

        protected override void InitializeSequence()
        {
            base.InitializeSequence();
            //초기화 시간 지정 - 0.5f

            Sequence sequence = DOTween.Sequence();
            sequence.Append(SpriteExtensions.SetSpritesColor(m_SpriteRenderers, 0.5f, true));
            sequence.Join(this.m_MovingBoard.defaultM.DOColor(Color.white, 0.5f));
            sequence.AppendInterval(0.5f).OnComplete(() =>
            {
                this.SetState(MachineState.READY);
            });
        }

        protected override void ReadySequence()
        {
            base.ReadySequence();
            if (!m_isStop)
                this.SetState(MachineState.STOP);
            else
                this.SetState(MachineState.PLAY);
        }

        protected override void PlaySequence()
        {
            base.PlaySequence();

            var sequence = DOTween.Sequence();
            this.SetBoardSpeed = this.Model.m_Data.Machine_Create_Speed * 0.9f;
            sequence.AppendInterval(this.Model.m_Data.Machine_Create_Speed * 0.5f).OnComplete(() =>
            {
                this.CheckMoney();
                this.CreateBaseMaterial();
                this.SetState(MachineState.MOVE);
            });
        }

        protected override void MoveSequence()
        {
            base.MoveSequence();

            this.m_MaterialObject.transform.DOLocalPath(this.m_Pos, this.Model.m_Data.Machine_Speed * 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                this.SetState(MachineState.WAIT);
            });
        }

        protected override void WaitSequence()
        {
            base.WaitSequence();
            this.SetBoardSpeed = 0;
            ThousandLinesManager.Instance.MachineReceive(this);
        }
        protected override void InSequence()
        {
            base.InSequence();
        }
        protected override void OutSequence()
        {
            base.OutSequence();
        }

        protected override void StopSequence()
        {
            base.StopSequence();
            this.m_stateSr.color = Color.red;
        }

        #endregion

        #region ObjectPool

        private void InitializeObjectPool()
        {
            this.materialObjectPool = new ObjectPool<MaterialObject>(
                 createFunc: () =>
                 {
                     var createMaterialObject = Instantiate(ThousandLinesManager.Instance.m_MaterialObject, this.transform);
                     createMaterialObject.poolToReturn = materialObjectPool;
                     return createMaterialObject;
                 },
                 actionOnGet: (materialObject) =>
                 {
                     this.SettingMaterialObject(materialObject);
                     materialObject.gameObject.SetActive(true);
                 }
                 ,
                 actionOnRelease: (materialObject) =>
                 {
                     materialObject.gameObject.SetActive(false);
                 },
                 actionOnDestroy: (materialObject) =>
                 {
                     Destroy(materialObject.gameObject);
                 }, maxSize: 100);
        }

        private void SettingMaterialObject(MaterialObject materialObject)
        {
            //생산 메터리올 오브젝트 세팅
            materialObject.gameObject.transform.SetParent(this.m_tr[0]);
            materialObject.gameObject.transform.position = this.m_tr[0].position;
            materialObject.SetMaterialObject(ThousandLinesManager.Instance.m_MaterialObject.Model.m_Data);
            //색상 복구
            materialObject.m_SpriteRenderer.color = Color.white;
            materialObject.transform.localScale = Vector3.one;
            //하위 오브젝트 비활성화
            for (int i = 0; i < materialObject.transform.childCount; i++)
            {
                if (materialObject.transform.GetChild(i) != null)
                {
                    materialObject.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region Others

        private void CheckMoney()
        {
            //생성 전 돈체크
            if (ThousandLinesManager.Instance.m_MaterialObject.Model.m_Data.Material_Price > ThousandLinesManager.Instance.Money)
            {
                ThousandLinesManager.Instance.m_MaterialObject = ThousandLinesManager.Instance.m_MaterialObjects[0];
                ThousandLinesUIManager.Instance.m_MaterialToggles[0].m_Toggle.isOn = true;
            }
            else
                ThousandLinesManager.Instance.Money = -ThousandLinesManager.Instance.m_MaterialObject.Model.m_Data.Material_Price;
        }

        private void CreateBaseMaterial()
        {
            var materialObject = materialObjectPool.Get();
            materialObject.transform.position = this.m_tr[0].position;

            materialObject.name = materialObject.name;
            materialObject.transform.localPosition = this.m_Pos[0];
            this.m_MaterialObject = materialObject;
        }

        #endregion
    }
}