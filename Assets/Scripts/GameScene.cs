using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Cysharp.Threading.Tasks;

namespace AssetData
{
	public class GameScene : MonoBehaviour
	{
		private void Start()
		{
			this.LoadAssetData();
		}

		private void LoadAssetData()
		{
			AssetDataManager.Load().ContinueWith(() =>
			{
				Debug.LogError("������ �ε� �Ϸ�");

				var test = AssetDataManager.GetData<LineData>(1);

				Debug.LogError(test.Line_Price);
			});
		}
	}
}