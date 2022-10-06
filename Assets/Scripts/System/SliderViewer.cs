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
		private TextMeshProUGUI m_Label;

		public Image Image
		{
			get { return this.m_Image; }
		}

		public TextMeshProUGUI Label
		{
			get { return this.m_Label; }
		}

		public float FillAmount
		{
			get { return this.m_Image.fillAmount; }
			set { this.m_Image.fillAmount = value; }
		}

		public string Text
		{
			set { this.m_Label.text = value; }
		}
	}
}