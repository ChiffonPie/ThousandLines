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
		private ActionSequence m_Sequence = new ActionSequence();

		[SerializeField]
		private SliderViewer m_LoadingSlider;
		[SerializeField]
		private float m_LoadingDuration = 2f;

		private void Awake()
        {
			this.InitializeGameScene();
		}

		private void InitializeGameScene()
        {
			this.SetupSequence();
			this.m_LoadingSlider.Image.fillAmount = 0;
			this.m_Sequence.Start();

		}

		/// <summary>
		/// �ε� ó��
		/// </summary>
		/// <param name="value"> �ε� Index </param>
		/// <param name="onCompleted"></param>
		private void Loading(float value, Action onCompleted = null)
		{
			//�ε� ��� �� ó��
			//��巹���� ������ �������� �ٿ�ε� �� ���,
			//AddressablesUtility.GetDownloadSizeAsync �� ���������, ��� ���⸸ ������
			float startValue = value / (float)this.m_Sequence.Count;
			float endValue = (value + 1) / (float)this.m_Sequence.Count;

			this.m_LoadingSlider.Label.DOGauge((int)Math.Ceiling(startValue * 100), 
											   (int)Math.Ceiling(endValue * 100),
											    this.m_LoadingDuration);
			this.m_LoadingSlider.Image.DOFillAmount(endValue, this.m_LoadingDuration).OnComplete(() =>
			 {
				 if (onCompleted != null)
					 onCompleted();
			 });
		}

		private void SetupSequence()
		{
			this.m_Sequence.Clear();
			this.m_Sequence.Add(this.LoadAssetData);
			this.m_Sequence.Add(this.LoadGameScene);
		}

		private void LoadAssetData()
		{
			this.SetGameSceneText = "Data Loading...";
			UniTask uniTask = AssetDataManager.Load().ContinueWith(() =>
			{
				this.Loading(0, () => 
				{
					this.SetGameSceneText = "Data Load Complete";
					this.m_Sequence.Next();
				});
			});
		}

		private void LoadGameScene()
		{
			this.SetGameSceneText = "Game Scene Loading...";
			this.Loading(1,() =>
			{
				this.SetGameSceneText = "Game Scene Load Complete";
				this.m_Sequence.Next();
			});
		}

		private string SetGameSceneText
		{
			set { m_GameSceneText.text = value; }
        }
	}
}