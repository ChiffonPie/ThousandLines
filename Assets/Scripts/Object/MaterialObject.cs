using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MaterialObject : MonoBehaviour
    {
        [SerializeField]
        private MaterialObjectModel Model = null;

        public List<int> LineTypes = new List<int>();

        [SerializeField]
        private new double m_Value = 0;

        public double Value
        {
            get { return m_Value; }
            set { this.m_Value = value; }
        }

        //처음 생성시 만들어지는 기본 데이터 메테리얼 오브젝트
        public void SetMaterialObject(MaterialObjectData materialObjectData)
        {
            var model = new MaterialObjectModel(materialObjectData);
            this.Model = model;
            this.Value = model.m_Data.Material_Value;
            //this.Value = materialObjectModel.m_data.Material_Value;
        }

        private void SetLineComplete(int lineindex)
        {
            if (this.Model == null) return;
            this.LineTypes.Add(lineindex);

            //금액 계산 처리
        }
    }
}