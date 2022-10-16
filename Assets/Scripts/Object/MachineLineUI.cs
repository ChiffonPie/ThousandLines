using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

namespace ThousandLines
{
    public class MachineLineUI : MonoBehaviour
    {
        public Image m_MachineImage;

        [SerializeField]
        public string m_MachineId;

        [SerializeField]
        private TextMeshProUGUI m_NameText;

        [SerializeField]
        private TextMeshProUGUI m_PriceText;

        [SerializeField]
        public Button m_Settingbutton;

        [SerializeField]
        private Button m_BuyButton;

        public void Initialize(MachineLine machineLine)
        {
            this.m_MachineId = machineLine.Model.m_Data.Id;
            this.m_NameText.text = machineLine.Model.m_Data.Line_Prosseing;
            this.m_PriceText.text = machineLine.Model.m_Data.Line_Price.ToString();
            this.m_Settingbutton.OnClickAsObservable().Subscribe(_ =>
            {
                this.OnClickSettingButton();
            });

            this.m_Settingbutton.interactable = machineLine.Model.m_Data.Line_Setting_Index == 0;

        }

        private void OnClickSettingButton()
        {
            this.m_Settingbutton.interactable = false;
            ThousandLinesManager.Instance.SettingMachine(this.m_MachineId);
        }
    }
}