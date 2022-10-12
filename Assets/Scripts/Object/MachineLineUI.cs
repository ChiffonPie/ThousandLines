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

        public void Initialize(MachineLineData machineLineData)
        {
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
                this.m_BuyButton.OnClickAsObservable().Subscribe(_ =>
                {
                    this.OnClickSettingButton();
                });
            }
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