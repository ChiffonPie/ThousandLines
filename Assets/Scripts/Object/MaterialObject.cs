using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThousandLines
{
    public class MaterialObject : MonoBehaviour
    {
        [SerializeField]
        private MaterialObjectModel m_MaterialObjectModel = null;

        [SerializeField]
        private double m_price = 0;

        private double Price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        //처음 생성시 만들어지는 기본 데이터 메테리얼 오브젝트
        private void SetMaterialType(MaterialObjectModel MaterialType)
        {
            this.m_MaterialObjectModel = MaterialType;
            this.Price = MaterialType.m_data.Material_Value;
        }

        private void SetLineComplete(int lineindex)
        {
            if (this.m_MaterialObjectModel == null) return;
            this.m_MaterialObjectModel.LineTypes.Add(lineindex);
        }
    }
}