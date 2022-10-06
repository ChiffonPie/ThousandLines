using DG.Tweening;
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

		public Sequence Fade(float endValue = 0, float duration = 1)
        {
			Sequence sequence = DOTween.Sequence();

			sequence.Append(this.Image.DOFade(endValue, duration));
			sequence.Join(this.m_Label.DOFade(endValue, duration));
			if (this.GetComponent<Image>() != null)
				sequence.Join(this.GetComponent<Image>().DOFade(endValue, duration));

			return sequence;
		}
	}
}