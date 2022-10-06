using UnityEngine;
using Cysharp.Threading.Tasks;
using ThousandLines_Data;
using DG.Tweening;
using TMPro;
using System;

namespace ThousandLines
{
	public class GameScene : MonoBehaviour
	{
		[SerializeField]
		private SliderViewer m_LoadingSlider;

		private ActionSequence m_Sequence = new ActionSequence();
		[SerializeField]
		private float m_LoadingDuration = 1f;

		private void Awake()
        {
			this.InitializeGameScene();
		}

        #region GameScene Initialize

        private void InitializeGameScene()
        {
			this.SetupSequence();
			this.InitaizeLoading();
			this.m_Sequence.Start(()=>
			{
				this.LoadComplete();
			});
		}

		private void InitaizeLoading()
		{
			this.m_LoadingSlider.Image.fillAmount = 0;
		}

        #endregion

        #region GaemScene Sequences

        /// <summary>
        /// initialize  할 시퀀스 추가
        /// </summary>
        private void SetupSequence()
		{
			this.m_Sequence.Clear();
			this.m_Sequence.Add(this.LoadAssetData);
			this.m_Sequence.Add(this.LoadGameScene);
		}

		private void LoadAssetData()
		{
			this.m_LoadingSlider.LoadingText.text = "Data Loading...";
			UniTask uniTask = AssetDataManager.Load().ContinueWith(() =>
			{
				this.Loading(this.m_Sequence.Index, () => 
				{
					this.m_LoadingSlider.LoadingText.text = "Data Load Complete";
					this.m_Sequence.Next();
				});
			});
		}

		private void LoadGameScene()
		{
			this.m_LoadingSlider.LoadingText.text = "Game Scene Loading...";
			this.Loading(this.m_Sequence.Index, () =>
			{
				this.m_LoadingSlider.LoadingText.text = "COMPLETE";
				this.m_Sequence.Next();
			});
		}

        #endregion

        private void LoadComplete()
        {
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.m_LoadingSlider.Fade(0, 0.3f));
			sequence.AppendCallback(() => { Debug.LogError("완료"); });
		}

		/// <summary>
		/// 로딩 처리
		/// </summary>
		/// <param name="value"> Sequence Index </param>
		/// <param name="onCompleted"></param>
		private void Loading(float value, Action onCompleted = null)
		{
			//로딩 계산 후 처리
			//어드레서블 에셋을 서버에서 다운로드 할 경우,
			//AddressablesUtility.GetDownloadSizeAsync 를 사용하지만, 기능 연출만 보여줌
			float startValue = value / (float)this.m_Sequence.Count;
			float endValue = (value + 1) / (float)this.m_Sequence.Count;

			this.m_LoadingSlider.PercentLabel.DOColor(Color.green, this.m_LoadingDuration);
			this.m_LoadingSlider.PercentLabel.DOGauge((int)Math.Ceiling(startValue * 100),
											   (int)Math.Ceiling(endValue * 100),
												this.m_LoadingDuration);
			this.m_LoadingSlider.Image.DOFillAmount(endValue, this.m_LoadingDuration).OnComplete(() =>
			{
				if (onCompleted != null)
					onCompleted();
			});
		}
	}
}