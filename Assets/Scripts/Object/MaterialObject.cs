using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;
using UnityEngine.Pool;

namespace ThousandLines
{
    public class MaterialObject : MonoBehaviour
    {
        public IObjectPool<MaterialObject> poolToReturn;

        [SerializeField]
        public MaterialObjectModel Model = null;

        public SpriteRenderer m_SpriteRenderer;

        //작업한 공정 리스트
        public List<int> processingList = new List<int>();
        private double m_Value = 0;

        private void Awake()
        {
            this.m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            this.processingList = new List<int>();
        }

        public double Value
        {
            get { return m_Value; }
            set { this.m_Value = value; }
        }

        //처음 생성시 만들어지는 기본 데이터 메테리얼 오브젝트
        public void SetMaterialObject(MaterialObjectData materialObjectData)
        {
            this.processingList.Clear();
            var model = new MaterialObjectModel(materialObjectData);
            this.Model = model;
            this.Value = model.m_Data.Material_Value;

            this.m_SpriteRenderer.sprite = Resources.Load<Sprite>($"{"Sprites/PNG/MaterialObjects/" + materialObjectData.Id}");
        }

        public void DestroyMaterialObject()
        {
            poolToReturn.Release(this);
        }
    }
}