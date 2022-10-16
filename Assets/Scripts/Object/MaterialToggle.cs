using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ThousandLines
{
    public class MaterialToggle : MonoBehaviour
    {
        public string Id;
        private int m_price;
        public Toggle m_Toggle;
        public Image m_Image;
        public TextMeshProUGUI m_PriceText;

        public int Price
        {
            get { return this.m_price; }
            set
            {
                this.m_price = value;
                this.m_PriceText.text = value.ToString();
            }
        }

        public void SetMaterial()
        {
            this.m_Image.sprite = Resources.Load<Sprite>($"{"Sprites/PNG/MaterialObjects/" + Id}");
        }
    }
}