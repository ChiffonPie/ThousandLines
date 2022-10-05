using System.Collections;
using System.Collections.Generic;
using ThousandLines_Data;
using UnityEngine;

namespace ThousandLines
{
    public abstract class MaterialObjectModel
    {
        // ������ ��
        public MaterialObjectData m_data;
        public List<int> LineTypes = new List<int>();
    }
}