using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThousandLines
{
	public class SliderViewer : MonoBehaviour
	{
		[SerializeField]
		private Image m_Image;

		[SerializeField]
		private Text m_Label;

		public Image Image
		{
			get { return this.m_Image; }
		}

		public Text Label
		{
			get { return this.m_Label; }
		}

		public float FillAmount
		{
			set { this.m_Image.fillAmount = value; }
		}

		public string Text
		{
			set { this.m_Label.text = value; }
		}

		public void SetValue(int value, int maxValue)
		{
			this.FillAmount = (float)value / maxValue;
			this.Text = $"{value}/{maxValue}";
		}
	}
}