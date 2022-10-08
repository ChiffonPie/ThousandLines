using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public class MachineLineModel
    {
        // 기계 데이터
        public MachineLineData m_Data;

        public MachineLineModel(MachineLineData data)
        {
            this.m_Data = data;
        }
    }
}