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
        private SpriteButton m_Settingbutton;

        [SerializeField]
        private SpriteButton m_BuyButton;

        public void Initialize(MachineLineData machineLineData)
        {
            //모든 버튼의 초기화
            this.SetButtonSprites(this.m_BuyButton, machineLineData);
            //구매 여부 체크
            if (machineLineData.Line_isGet == 0)
            {
                // 돈주고 살 수 있는 버튼 이벤트 추가
                this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickBuyButton();
                });
                return;
            }

            //설치 여부 체크
            if (machineLineData.Line_isActive == 0)
            {
                // 설치할 수 있는 버튼 이벤트 추가
                this.m_Settingbutton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }
        }

        private void SetButtonSprites(SpriteButton button, MachineLineData machineLineData)
        {
            //리소스 로드로 스프라이트 세팅을 함

            button.NormalSprite = null;
            button.spriteState = new SpriteState()
            {
                highlightedSprite = null,
                pressedSprite     = null,
                selectedSprite    = null,
                disabledSprite    = null,
            };
        }

        private void OnClickBuyButton()
        {
            Debug.LogError(11);
        }

        private void OnClickSettingButton()
        {
            Debug.LogError(22);
        }
    }
}