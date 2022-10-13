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
        private string m_MachineId;

        [SerializeField]
        private Image m_MachineImage;
        [SerializeField]
        private TextMeshProUGUI m_Price;

        [SerializeField]
        private SpriteButton m_Settingbutton;

        [SerializeField]
        private SpriteButton m_BuyButton;

        public void Initialize(MachineLine machineLine)
        {
            //모든 버튼의 초기화
            //this.SetButtonSprites(this.m_BuyButton, machineLineData);

            //일단 설치부터
            //this.m_BuyButton.gameObject.SetActive(machineLineData.Line_isGet == 0);
            //구매 여부 체크
            //if (machineLineData.Line_isGet == 0)
            //{
            //    // 돈주고 살 수 있는 버튼 이벤트 추가
            //    this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
            //    {
            //        this.OnClickBuyButton();
            //    });
            //}

            this.m_MachineId = machineLine.Model.m_Data.Id;
            //설치 여부 체크
            if (machineLine.Model.m_Data.Line_Setting_Index == 0)
            {
                // 설치할 수 있는 버튼 이벤트 추가
                this.m_Settingbutton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }

            this.m_Settingbutton.interactable = machineLine.Model.m_Data.Line_Setting_Index == 0;
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
            Debug.LogError("설치함");
            this.m_Settingbutton.interactable = false;

            //설치 메커니즘
            ThousandLinesManager.Instance.SettingMachine(this.m_MachineId);
            //설치 예약에 들어갑니다. (해제 불가)
            //리스트에 추가됩니다.
        }
    }
}