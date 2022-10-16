using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;

namespace DG.Tweening
{
	public static class DOTweenExtensions
	{
		public static TweenerCore<int, int, NoOptions> DOGauge(this TMP_Text target, int fromValue, int endValue, float duration)
		{
			return DOTween.To(() => fromValue, x => target.text = $"{x} %", endValue, duration).SetTarget(target);
		}
	}
}