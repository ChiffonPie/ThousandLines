using UnityEngine;
using UnityEngine.UI;

namespace ThousandLines
{
    public class MaterialToggle : MonoBehaviour
    {
        public string Id;
        public Toggle m_Toggle;
        public Image m_Image;

        public void SetMaterialSprite()
        {
            this.m_Image.sprite = Resources.Load<Sprite>($"{"Sprites/PNG/MaterialObjects/" + Id}");
        }
    }
}