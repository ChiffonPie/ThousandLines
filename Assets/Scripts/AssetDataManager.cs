using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace AssetData
{
	public class AssetDataManager : MonoBehaviour
	{
		public static AssetDataManager Instance { get; private set; }

		private string dataPath = "Assets/Data"; // 데이터 경로
		private static Dictionary<Type, IDictionary> AssetMap = new Dictionary<Type, IDictionary>();

		public static string RemotePath { get; private set; }

		public static string CachePath { get; private set; }

		public static bool IsLoaded { get; private set; }

		private void Start()
		{
			this.Load();
		}

		public void Load()
		{
			Instance = this;

			var test = CSVtoJSON.ConvertCsvFileToJsonObject(dataPath);
			
			AssetDataManager.AssetMap.Clear();
            UniTask uniTask = AssetDataManager.Load<int, LineData>(test);
		}


		private static async UniTask Load<TKey, TValue>(string name) where TValue : AssetData<TKey>, new()
		{
			AssetDataManager.LoadJson<TKey, TValue>(name);
		}

		private static void LoadJson<TKey, TValue>(string jsonText) where TValue : AssetData<TKey>, new()
		{
			var array = JArray.Parse(jsonText);
			var dataMap = new Dictionary<TKey, TValue>(array.Count);

			foreach (var token in array)
			{
				var data = token.ToObject<TValue>(new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });

				if (data != null)
				{
					data.Deserialize(token as JObject);
					dataMap.Add(data.Id, data);
				}
				else
				{
					throw new NullReferenceException();
				}
			}

			AssetDataManager.AssetMap.Add(typeof(TValue), dataMap);
		}

		#region NestedClass - 간단한 데이터 구조 설정
		public class LineData : AssetData<int>, ISort
		{
			[JsonProperty("Line_Price")]
			public double Line_Price { get; protected set; }

			[JsonProperty("Line_Speed")]
			public float Line_Speed { get; protected set; }

			[JsonProperty("Line_Order_index")]
			public int OrderIndex { get; private set; }
		}

		#endregion
	}


}