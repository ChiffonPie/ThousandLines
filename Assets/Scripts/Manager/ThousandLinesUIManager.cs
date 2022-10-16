using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ThousandLines
{
    public class ThousandLinesUIManager : MonoBehaviour
    {
        public TextMeshProUGUI m_moneyText;
        [SerializeField]
        private GameObject m_GameUI;

        [Header(" [ MachineButton ] ")]
        [SerializeField]
        private Transform m_MachineButtonTr;
        [SerializeField]
        private MachineLineUI m_MachineButtonPreview;
        [SerializeField]
        private List<MachineLineUI> m_MachineButtonUIs;

        [Header(" [ Material Toggle ] ")]
        [SerializeField]
        private UnityEngine.UI.ToggleGroup m_MaterialToggleTr;
        [SerializeField]
        private MaterialToggle m_MaterialTogglePreview;

        public List<MaterialToggle> m_MaterialToggles;

        public static ThousandLinesUIManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            this.SetAcitveGameUI(false);
        }

        // UI 기본세팅
        public void SetAcitveGameUI(bool isActive)
        {
            this.m_GameUI.SetActive(isActive);
        }

        //게임 매니저가 Init
        #region Initialize
        public void Initialize(List<MachineLine> machineLines)
        {
            for (int i = 0; i < machineLines.Count; i++)
            {
                this.CreateMachineButtonUI(machineLines[i]);
            }
        }

        public void InitializeMaterialToggles(List<MaterialObject> materialObjects)
        {
            for (int i = 0; i < materialObjects.Count; i++)
            {
                this.CreateMaterialObjectToggle(materialObjects[i]);
            }
        }

        #endregion

        #region Create Settings

        //머신 생성버튼 세팅
        private void CreateMachineButtonUI(MachineLine machineLine)
        {
            MachineLineUI machineLineUI = Instantiate(this.m_MachineButtonPreview, this.m_MachineButtonTr);
            machineLineUI.name = machineLine.Model.m_Data.Id;
            machineLineUI.Initialize(machineLine);
            machineLineUI.m_MachineImage.sprite = Resources.Load<Sprite>($"{"Sprites/PNG/UI/Machine_Line_UI/" + machineLine.Model.m_Data.Id}");
            this.m_MachineButtonUIs.Add(machineLineUI);
        }

        //토글 선택버튼 세팅
        private void CreateMaterialObjectToggle(MaterialObject materialObject)
        {
            MaterialToggle materialToggle = Instantiate(this.m_MaterialTogglePreview, this.m_MaterialToggleTr.transform);
            materialToggle.m_Toggle.group = this.m_MaterialToggleTr;
            materialToggle.Id = materialObject.Model.m_Data.Id;
            materialToggle.SetMaterial();
            materialToggle.Price = materialObject.Model.m_Data.Material_Price;
            materialToggle.m_Toggle.onValueChanged.AddListener(HandleToggleValueChanged);

            m_MaterialToggles.Add(materialToggle);
        }

        #endregion

        #region Others

        //머신 버튼 활성화 처리
        public void SetInteractableButton(string id)
        {
            for (int i = 0; i < this.m_MachineButtonUIs.Count; i++)
            {
                if (this.m_MachineButtonUIs[i].m_MachineId == id)
                {
                    this.m_MachineButtonUIs[i].m_Settingbutton.interactable = true;
                }
            }
        }

        //토글 이벤트 체크
        private void HandleToggleValueChanged(bool isOn)
        {
            if (isOn)
                this.GetSelectedToggle();
        }

        private void GetSelectedToggle()
        {
            for (int i = 0; i < this.m_MaterialToggles.Count; i++)
            {
                if (this.m_MaterialToggles[i].m_Toggle.isOn)
                {
                    //돈 소모
                    if (this.m_MaterialToggles[i].Price <= ThousandLinesManager.Instance.Money)
                    {
                        ThousandLinesManager.Instance.m_MaterialObject = ThousandLinesManager.Instance.m_MaterialObjects[i];
                        return;
                    }

                    this.m_MaterialToggles[0].m_Toggle.isOn = true;
                    break;
                }
            }
        }

        #endregion
    }
}