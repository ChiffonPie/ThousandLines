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
		/// initialize  �� ������ �߰�
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
		/// �ε� ó��
		/// </summary>
		/// <param name="value"> Sequence Index </param>
		/// <param name="onCompleted"></param>
		private void Loading(float value, Action onCompleted = null)
		{
			//�ε� ��� �� ó��
			//��巹���� ������ �������� �ٿ�ε� �� ���,
			//AddressablesUtility.GetDownloadSizeAsync �� ���������, ��� ���⸸ ������
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

			//������ Ƚ��
			for (int i = 0; i < 4; i++)
            {
				sequence.Append(this.m_GameSceneText.DOColor(Color.black, this.m_LoadingDuration / 10));
				sequence.Append(this.m_GameSceneText.DOColor(Color.green, this.m_LoadingDuration / 10));
			}

			sequence.Append(this.m_GameSceneText.DOFade(0, this.m_LoadingDuration / 5));
			sequence.Join(this.m_LoadingSlider.Fade(0, this.m_LoadingDuration / 5));
			sequence.AppendCallback(() => { Debug.LogError("�Ϸ�"); });
		}

		private string SetGameSceneText
		{
			set { m_GameSceneText.text = value; }
        }
	}
}