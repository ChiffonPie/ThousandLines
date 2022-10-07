using UnityEngine;
using Cysharp.Threading.Tasks;
using ThousandLines_Data;
using DG.Tweening;
using System;
using TMPro;

namespace ThousandLines
{
	public class GameScene : MonoBehaviour
	{
		[SerializeField]
		private SliderViewer m_LoadingSlider;

		private ActionSequence m_Sequence = new ActionSequence();

		[SerializeField]
		private TextMeshProUGUI m_IntroLabel;
		[SerializeField]
		private float m_LoadingDuration = 0.5f;

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
			this.m_LoadingSlider.gameObject.SetActive(true);
			this.m_LoadingSlider.Image.fillAmount = 0;
		}

        #endregion

        #region GaemScene Sequences

        /// <summary>
        /// Initialize  �� ������ �߰�
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

		#region GameScene Load

		/// <summary>
		/// �ε� ó��
		/// </summary>
		/// <param name="value"> Sequence Index </param>
		/// <param name="onCompleted"></param>
		private void Loading(float value, Action onCompleted = null)
		{
			//�ε� ��� �� ó��
			//��巹���� ������ �������� �ٿ�ε� �� ���,
			//AddressablesUtility.GetDownloadSizeAsync �� ���������, ��� ���⸸ ������
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
		private void LoadComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.m_LoadingSlider.Fade(0, 0.3f));
			sequence.AppendCallback(() => {
				this.m_LoadingSlider.gameObject.SetActive(false);
				this.IntroLabel();
			});
		}

		#endregion

		#region GameScene Intro
		private void IntroLabel()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Append(this.m_IntroLabel.transform.DOMove(new Vector2(this.m_IntroLabel.transform.position.x + 50, this.m_IntroLabel.transform.position.y - 25), 0.4f, true));
			sequence.Join(this.m_IntroLabel.DOColor(Color.white, this.m_LoadingDuration * 0.4f));
			sequence.AppendInterval(this.m_LoadingDuration);
			sequence.Append(this.m_IntroLabel.DOColor(Color.black, 0.4f));
			sequence.Join(this.m_IntroLabel.transform.DOMove(new Vector2(this.m_IntroLabel.transform.position.x - 50, this.m_IntroLabel.transform.position.y + 25), this.m_LoadingDuration * 0.4f, true));
			sequence.AppendCallback(() => 
			{
				ThousandLinesManager.Instance.Initiaize();
			});
		}

		#endregion
	}
}