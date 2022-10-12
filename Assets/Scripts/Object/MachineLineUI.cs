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
            //��� ��ư�� �ʱ�ȭ
            this.SetButtonSprites(this.m_BuyButton, machineLineData);
            //���� ���� üũ
            if (machineLineData.Line_isGet == 0)
            {
                // ���ְ� �� �� �ִ� ��ư �̺�Ʈ �߰�
                this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickBuyButton();
                });
                return;
            }

            //��ġ ���� üũ
            if (machineLineData.Line_isActive == 0)
            {
                // ��ġ�� �� �ִ� ��ư �̺�Ʈ �߰�
                this.m_Settingbutton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }
        }

        private void SetButtonSprites(SpriteButton button, MachineLineData machineLineData)
        {
            //���ҽ� �ε�� ��������Ʈ ������ ��

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