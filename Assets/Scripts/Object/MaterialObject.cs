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
        private double m_price = 0;

        private double Price
        {
            get { return m_price; }
            set { this.m_price = value; }
        }

        //ó�� ������ ��������� �⺻ ������ ���׸��� ������Ʈ
        private void SetMaterialObject(MaterialObjectModel materialObjectModel)
        {
            this.Model = materialObjectModel;
            this.Price = materialObjectModel.m_data.Material_Value;
        }

        private void SetLineComplete(int lineindex)
        {
            if (this.Model == null) return;
            this.Model.LineTypes.Add(lineindex);

            //�ݾ� ��� ó��
        }
    }
}