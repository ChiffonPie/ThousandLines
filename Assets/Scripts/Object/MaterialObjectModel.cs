using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MaterialObjectModel
    {
        // 원자재 모델
        public MaterialObjectData m_Data;
        public MaterialObjectModel(MaterialObjectData data)
        {
            this.m_Data = data;
        }
    }
}