using UnityEngine;
using Cysharp.Threading.Tasks;
using ThousandLines_Data;

namespace ThousandLines
{
	public class GameScene : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.UI.Text m_GameSceneText;
		private ActionSequence m_Sequence = new ActionSequence();
        
		private void Awake()
        {
			this.InitializeGameScene();
		}

		private void InitializeGameScene()
        {
			this.SetupSequence();
			this.m_Sequence.Start();
		}

		private void SetupSequence()
		{
			this.m_Sequence.Clear();
			this.m_Sequence.Add(this.LoadAssetData);
			this.m_Sequence.Add(this.LoadGameScene);
		}

		private void LoadAssetData()
		{
			UniTask uniTask = AssetDataManager.Load().ContinueWith(() =>
			{
				this.SetGameSceneText = "데이터 로드 완료";
				var test = AssetDataManager.GetData<MachineLineData>(1);

				Debug.LogError(test.Machine_isActive);
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