using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ThousandLines_Data;
using UniRx;

namespace ThousandLines
{
    public class MachineLineUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_MachineImage;
        [SerializeField]
        private TextMeshProUGUI m_Price;

        [SerializeField]
        private Button m_BuyButton;
        [SerializeField]
        private Button m_Settingbutton;

        public void Initialize(MachineLineData machineLineDatas)
        {
            //구매 여부 체크
            if (machineLineDatas.Line_isGet == 0)
            {
                // 돈주고 살 수 있는 버튼 이벤트 추가
                this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickBuyButton();
                });
                return;
            }

            //설치 여부 체크
            if (machineLineDatas.Line_isActive == 0)
            {
                // 설치할 수 있는 버튼 이벤트 추가
                this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }
        }


        private void OnClickBuyButton()
        {

        }


        private void OnClickSettingButton()
        {

        }
    }
}