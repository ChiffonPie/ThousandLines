using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MaterialObject : MonoBehaviour
    {
        [SerializeField]
        public MaterialObjectModel Model = null;

        public SpriteRenderer m_SpriteRenderer;

        public List<int> LineTypes = new List<int>();
        private double m_Value = 0;

        private void Awake()
        {
            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public double Value
        {
            get { return m_Value; }
            set { this.m_Value = value; }
        }

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        public void SetMaterialObject(MaterialObjectData materialObjectData)
        {
            var model = new MaterialObjectModel(materialObjectData);
            this.Model = model;
            this.Value = model.m_Data.Material_Value;

            this.m_SpriteRenderer.sprite = Resources.Load<Sprite>($"{"Sprites/PNG/MaterialObjects/" + materialObjectData.Id}");
        }

        private void SetLineComplete(int lineindex)
        {
            if (this.Model == null) return;
            this.LineTypes.Add(lineindex);
            //�ݾ� ��� ó��
        }
    }
}