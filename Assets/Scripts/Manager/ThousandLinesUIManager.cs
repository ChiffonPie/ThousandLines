using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ThousandLines_Data;

namespace ThousandLines
{
    public class ThousandLinesUIManager : MonoBehaviour
    {
        public TextMeshProUGUI m_moneyText;
        [SerializeField]
        private GameObject m_GameUI;
        [SerializeField]
        private Transform m_MachineButtonTr;
        [SerializeField]
        private MachineLineUI m_MachineButtonPreview;

        [SerializeField]
        private List<MachineLineUI> m_MachineButtonUIs;

        public static ThousandLinesUIManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            this.SetAcitveGameUI(false);
        }

        //게임 매니저가 Init 해줌
        public void Initialize(List<MachineLine> machineLines)
        {
            for (int i = 0; i < machineLines.Count; i++)
            {
                this.CreateMachineButtonUI(machineLines[i]);
            }
        }

        public void SetAcitveGameUI(bool isActive)
        {
            this.m_GameUI.SetActive(isActive);
        }

        private void CreateMachineButtonUI(MachineLine machineLine)
        {
            MachineLineUI machineLineUI = Instantiate(m_MachineButtonPreview, m_MachineButtonTr);
            machineLineUI.Initialize(machineLine);
            this.m_MachineButtonUIs.Add(machineLineUI);
        }
    }
}