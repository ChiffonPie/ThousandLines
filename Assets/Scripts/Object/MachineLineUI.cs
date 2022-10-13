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
        private int m_MachineIndex;

        [SerializeField]
        private Image m_MachineImage;
        [SerializeField]
        private TextMeshProUGUI m_Price;

        [SerializeField]
        private SpriteButton m_Settingbutton;

        [SerializeField]
        private SpriteButton m_BuyButton;


        public int Index
        {
            get { return this.m_MachineIndex; }
            //set { this.m_MachineIndex = value; }
        }

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

            this.m_MachineIndex = machineLine.Index;
            //��ġ ���� üũ
            if (machineLine.Model.m_Data.Line_isActive == 0)
            {
                // ��ġ�� �� �ִ� ��ư �̺�Ʈ �߰�
                this.m_Settingbutton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }

            if (machineLine.Model.m_Data.Line_isActive == 0)
            {
                this.m_Settingbutton.interactable = true;
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
            Debug.LogError("��ġ��");
            this.m_Settingbutton.interactable = false;

            //ThousandLinesManager.Instance.m_Machines.Add();

            //��ġ ���࿡ ���ϴ�. (���� �Ұ�)
            //����Ʈ�� �߰��˴ϴ�.
        }
    }
}