using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ThousandLines_Data;

namespace ThousandLines
{
    public class MachineLineUI : MonoBehaviour
    {
        public Button m_button;
        public TextMeshProUGUI m_Price;

        public void Initialize(MachineLineData machineLineDatas)
        {
            this.m_button.interactable = machineLineDatas.Line_isActive == 1;
        }
    }
}