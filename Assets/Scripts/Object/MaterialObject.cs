using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThousandLines
{
    public class MaterialObject : MonoBehaviour
    {
        [SerializeField]
        private MaterialObjectModel Model = null;

        [SerializeField]
        private double m_Value = 0;

        public double Value
        {
            get { return m_Value; }
            set { this.m_Value = value; }
        }

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        private void SetMaterialObject(MaterialObjectModel materialObjectModel)
        {
            this.Model = materialObjectModel;
            this.Value = materialObjectModel.m_data.Material_Value;
        }

        private void SetLineComplete(int lineindex)
        {
            if (this.Model == null) return;
            this.Model.LineTypes.Add(lineindex);

            //�ݾ� ��� ó��
        }
    }
}