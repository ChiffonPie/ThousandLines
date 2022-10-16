using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

namespace ThousandLines
{
    public class MachineLineUI : MonoBehaviour
    {
        [SerializeField]
        public string m_MachineId;

        [SerializeField]
        private Image m_MachineImage;
        [SerializeField]
        private TextMeshProUGUI m_Price;

        [SerializeField]
        public Button m_Settingbutton;

        [SerializeField]
        private Button m_BuyButton;

        public void Initialize(MachineLine machineLine)
        {
            this.m_MachineId = machineLine.Model.m_Data.Id;
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