using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ThousandLines_Data
{
	public class AssetDataManager
	{
		public static string dataPath = "Assets/Data"; // 데이터 경로
		private static Dictionary<Type, IDictionary> AssetMap = new Dictionary<Type, IDictionary>();
		public static bool IsLoaded { get; private set; }

		public static async UniTask Load()
		{
			AssetDataManager.IsLoaded = false;

			AssetDataManager.AssetMap.Clear();
			await UniTask.WhenAll(new List<UniTask>()
			{
				AssetDataManager.Load<int, UserData>("UserData"),
				AssetDataManager.Load<int, BaseMachineData>("BaseMachineData"),
				AssetDataManager.Load<string, MachineLineData>("MachineLineData"),
			});

			AssetDataManager.IsLoaded = true;
		}

		private static async UniTask Load<TKey, TValue>(string name) where TValue : AssetData<TKey>, new()
		{
			var jsonData = CSVtoJSON.ConvertCsvFileToJson(name);
			AssetDataManager.LoadJson<TKey, TValue>(jsonData);
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

		#region GetData
		public static TValue GetData<TValue>(string id) where TValue : class, IAssetData
		{
			if (string.IsNullOrEmpty(id))
				return null;

			return AssetDataManager.GetData<string, TValue>(id);
		}

		public static TValue GetData<TValue>(short id) where TValue : class, IAssetData
		{
			return AssetDataManager.GetData<short, TValue>(id);
		}

		public static TValue GetData<TValue>(int id) where TValue : class, IAssetData
		{
			return AssetDataManager.GetData<int, TValue>(id);
		}

		public static TValue GetData<TValue>(long id) where TValue : class, IAssetData
		{
			return AssetDataManager.GetData<long, TValue>(id);
		}

		public static TValue GetData<TKey, TValue>(TKey id) where TValue : class, IAssetData
		{
			if (AssetDataManager.AssetMap.TryGetValue(typeof(TValue), out var dictionary))
			{
				var dataMap = dictionary as Dictionary<TKey, TValue>;
				if (dataMap != null && dataMap.TryGetValue(id, out var data))
					return data;
			}

			return null;
		}

		public static TValue GetData<TValue>(Func<TValue, bool> listener) where TValue : class, IAssetData
		{
			if (AssetDataManager.AssetMap.TryGetValue(typeof(TValue), out var dataMap))
			{
				var values = dataMap.Values;

				foreach (TValue data in values)
				{
					var isValid = listener(data);
					if (isValid)
						return data;
				}
			}

			return null;
		}

		public static List<TValue> GetDatas<TValue>(Func<TValue, bool> listener = null) where TValue : IAssetData
		{
			var type = typeof(TValue);
			var list = new List<TValue>();

			if (AssetDataManager.AssetMap.TryGetValue(type, out var dictionary))
			{
				var values = dictionary.Values;

				foreach (TValue data in values)
				{
					if (listener != null)
					{
						var isValid = listener(data);
						if (isValid)
							list.Add(data);
					}
					else
					{
						list.Add(data);
					}
				}
			}
			return list;
		}
		#endregion
	}
}