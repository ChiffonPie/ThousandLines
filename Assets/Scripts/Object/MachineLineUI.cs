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
            //��� ��ư�� �ʱ�ȭ
            //this.SetButtonSprites(this.m_BuyButton, machineLineData);

            //�ϴ� ��ġ����
            //this.m_BuyButton.gameObject.SetActive(machineLineData.Line_isGet == 0);
            //���� ���� üũ
            //if (machineLineData.Line_isGet == 0)
            //{
            //    // ���ְ� �� �� �ִ� ��ư �̺�Ʈ �߰�
            //    this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
            //    {
            //        this.OnClickBuyButton();
            //    });
            //}

            this.m_MachineId = machineLine.Model.m_Data.Id;
            //��ġ ���� üũ
            if (machineLine.Model.m_Data.Line_Setting_Index == 0)
            {
                // ��ġ�� �� �ִ� ��ư �̺�Ʈ �߰�
                this.m_Settingbutton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }

            this.m_Settingbutton.interactable = machineLine.Model.m_Data.Line_Setting_Index == 0;
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
            Debug.LogError("��ġ��");
            this.m_Settingbutton.interactable = false;

            //��ġ ��Ŀ����
            ThousandLinesManager.Instance.SettingMachine(this.m_MachineId);
            //��ġ ���࿡ ���ϴ�. (���� �Ұ�)
            //����Ʈ�� �߰��˴ϴ�.
        }
    }
}