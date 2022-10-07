using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class BaseMachineModel
    {
        // 기계 데이터
        public BaseMachineData m_Data;

        public BaseMachineModel(BaseMachineData data)
        {
            this.m_Data = data;
        }
    }
}