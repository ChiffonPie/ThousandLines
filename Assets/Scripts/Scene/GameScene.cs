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
		private TextMeshProUGUI m_GameSceneText;
		[SerializeField]
		private SliderViewer m_LoadingSlider;

		private ActionSequence m_Sequence = new ActionSequence();
		[SerializeField]
		private float m_LoadingDuration = 1f;

		private void Awake()
        {
			this.InitializeGameScene();
		}

		private void InitializeGameScene()
        {
			this.SetupSequence();
			this.InitaizeLoading();
			this.m_Sequence.Start(()=>
			{
				this.LoadComplete();
			});
		}


		/// <summary>
		/// initialize  할 시퀀스 추가
		/// </summary>
		private void SetupSequence()
		{
			this.m_Sequence.Clear();
			this.m_Sequence.Add(this.LoadAssetData);
			this.m_Sequence.Add(this.LoadGameScene);
		}

		private void InitaizeLoading()
        {
			this.m_LoadingSlider.Image.fillAmount = 0;
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
			float startValue =  value      / (float)this.m_Sequence.Count;
			float endValue   = (value + 1) / (float)this.m_Sequence.Count;


			this.m_LoadingSlider.Label.DOColor(Color.green, this.m_LoadingDuration);
			this.m_LoadingSlider.Label.DOGauge((int)Math.Ceiling(startValue * 100),
											   (int)Math.Ceiling(endValue   * 100),
											    this.m_LoadingDuration);
			this.m_LoadingSlider.Image.DOFillAmount(endValue, this.m_LoadingDuration).OnComplete(() =>
			 {
				 if (onCompleted != null)
					 onCompleted();
			 });
		}

		private void LoadAssetData()
		{
			this.SetGameSceneText = "Data Loading...";
			UniTask uniTask = AssetDataManager.Load().ContinueWith(() =>
			{
				this.Loading(this.m_Sequence.Index, () => 
				{
					this.SetGameSceneText = "Data Load Complete";
					this.m_Sequence.Next();
				});
			});
		}

		private void LoadGameScene()
		{
			this.SetGameSceneText = "Game Scene Loading...";
			this.Loading(this.m_Sequence.Index, () =>
			{
				this.SetGameSceneText = "Game Scene Load Complete";
				this.m_Sequence.Next();
			});
		}

		private void LoadComplete()
        {
			Sequence sequence = DOTween.Sequence();

			//깜빡임 횟수
			for (int i = 0; i < 4; i++)
            {
				sequence.Append(this.m_GameSceneText.DOColor(Color.black, this.m_LoadingDuration / 10));
				sequence.Append(this.m_GameSceneText.DOColor(Color.green, this.m_LoadingDuration / 10));
			}

			sequence.Append(this.m_GameSceneText.DOFade(0, this.m_LoadingDuration / 5));
			sequence.Join(this.m_LoadingSlider.Fade(0, this.m_LoadingDuration / 5));
			sequence.AppendCallback(() => { Debug.LogError("완료"); });
		}

		private string SetGameSceneText
		{
			set { m_GameSceneText.text = value; }
        }
	}
}