using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ThousandLines
{
	public class GameScene : MonoBehaviour
	{
		private ActionSequence m_Sequence = new ActionSequence();
        
		private void Awake()
        {
            this.SetupSequence();
		}

        private void Start()
		{
			this.m_Sequence.Start();
		}


		private void SetupSequence()
		{
			this.m_Sequence.Clear();
			this.m_Sequence.Add(this.LoadAssetData);
		}

		private void LoadAssetData()
		{
            UniTask uniTask = AssetDataManager.Load().ContinueWith(() =>
			{
				Debug.LogError("데이터 로드 완료");
				var test = AssetDataManager.GetData<LineData>(1);
				Debug.LogError(test.Line_Price);


				var test2 = AssetDataManager.GetData<UserData>(1);
				Debug.LogError(test2.Line_Price);
			});
		}
	}
}