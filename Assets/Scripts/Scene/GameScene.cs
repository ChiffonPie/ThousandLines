using UnityEngine;
using Cysharp.Threading.Tasks;
using ThousandLines_Data;
using DG.Tweening;

namespace ThousandLines
{
	public class GameScene : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.UI.Text m_GameSceneText;
		private ActionSequence m_Sequence = new ActionSequence();

		[SerializeField]
		private SliderViewer m_LoadingSlider;


		private void Awake()
        {
			this.InitializeGameScene();
		}

		private void InitializeGameScene()
        {
			this.SetupSequence();
			this.SetupLoading();
			this.m_Sequence.Start();
		}

		private void SetupLoading()
        {
			//로딩 계산 후 처리
			//어드레서블 에셋을 서버에서 다운로드 할 경우,
			//AddressablesUtility.GetDownloadSizeAsync 를 사용하지만, 기능 연출만 보여줌
			var tweenParams = new TweenParams();
			tweenParams.SetRelative();
			tweenParams.SetEase(Ease.InQuad);

			this.m_LoadingSlider.Image.fillAmount = 0;
			this.m_LoadingSlider.Image.DOFillAmount(1,10).SetAs(tweenParams);
			//this.m_LoadingSlider.Label.DOGauge(0, 1, 0 ,1).SetAs(tweenParams).OnComplete(() =>
			//{
			//	AudioManager.Instance.Stop("sfx_symbol_add_short_more");
			//
			//	if (onCompleted != null)
			//		onCompleted();
			//});
		}

		private void SetupSequence()
		{
			this.m_Sequence.Clear();
			this.m_Sequence.Add(this.LoadAssetData);
			this.m_Sequence.Add(this.LoadGameScene);

			//this.m_Sequence.Count
		}

		private void LoadAssetData()
		{
			UniTask uniTask = AssetDataManager.Load().ContinueWith(() =>
			{
				this.SetGameSceneText = "데이터 로드 완료";
				this.m_Sequence.Next();
			});
		}

		private void LoadGameScene()
        {
			this.SetGameSceneText = "Game Scene 로드 완료";
			this.m_Sequence.Next();
		}

		private string SetGameSceneText
		{
			set { m_GameSceneText.text = value; }
        }
	}
}