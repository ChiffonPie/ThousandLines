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
		private TextMeshProUGUI m_LoadingText;

		[SerializeField]
		private TextMeshProUGUI m_PercentLabel;

		public Image Image
		{
			get { return this.m_Image; }
		}

		public TextMeshProUGUI PercentLabel
		{
			get { return this.m_PercentLabel; }
		}

		public float FillAmount
		{
			get { return this.m_Image.fillAmount; }
			set { this.m_Image.fillAmount = value; }
		}

		public TextMeshProUGUI LoadingText
		{
			get { return this.m_LoadingText; }
		}

		public Sequence Fade(float endValue = 0, float duration = 1)
        {
			Sequence sequence = DOTween.Sequence();

			//±ôºýÀÓ È½¼ö
			for (int i = 0; i < 3; i++)
			{
				sequence.Append(this.m_LoadingText.DOColor(Color.black, 0.09f));
				sequence.Append(this.m_LoadingText.DOColor(Color.green, 0.09f));
			}

			sequence.AppendInterval(0.5f);
			sequence.Append(this.Image.DOFade(endValue, duration));
			sequence.Join(this.m_PercentLabel.DOFade(endValue, duration));
			sequence.Join(this.LoadingText.DOFade(endValue, duration));
			if (this.GetComponent<Image>() != null)
				sequence.Join(this.GetComponent<Image>().DOFade(endValue, duration));

			sequence.Join(this.transform.DOMove(new Vector2(this.transform.position.x - 50, this.transform.position.y + 25), 0.2f, true));

			return sequence;
		}
	}
}