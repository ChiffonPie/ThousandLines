using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace AssetData
{
    public class CSVtoJSON
    {
        //public string jsonValue = "Assets/Data";

        //특정 경로의 데이터를 전체를 파싱하는 방식 (서버 패킷 구조와 동일하도록)
        public static string ConvertAllCsvFileToJson()
        {
            List<JObject> jsonData = new List<JObject>();
            string[] files = Directory.GetFiles($"{AssetDataManager.Instance.dataPath}", "*.csv");
            string jsonAllData = null;

            foreach (var file in files)
            {
                var csv = new List<string[]>();
                var lines = File.ReadAllLines(file);

                foreach (string line in lines)
                    csv.Add(line.Split(','));

                var properties = lines[0].Split(',');
                for (int i = 1; i < lines.Length; i++)
                {
                    JObject json = new JObject();
                    for (int j = 0; j < properties.Length; j++)
                    {
                        json.Add(properties[j], csv[i][j]);
                    }
                    jsonData.Add(json);
                }
            }

            // Json Data Array
            for (int i = 0; i < jsonData.Count; i++)
            {
                jsonAllData += jsonData[i].ToString();
                if (i != jsonData.Count - 1) jsonAllData += ",";
            }

            UnityEngine.Debug.LogError(jsonAllData);

            return jsonAllData;
        }


        //특정 파일을 파싱하는 방식
        public static string ConvertCsvFileToJson(string fileName)
        {
            List<JObject> jsonData = new List<JObject>();
            string[] files = Directory.GetFiles($"{AssetDataManager.Instance.dataPath}", "*.csv");
            string jsonAllData = null;

            foreach (var file in files)
            {
                if (Path.GetFileNameWithoutExtension(file) == fileName)
                {
                    jsonAllData = "[";
                    var csv = new List<string[]>();
                    var lines = File.ReadAllLines(file);

                    foreach (string line in lines)
                        csv.Add(line.Split(','));

                    var properties = lines[0].Split(',');
                    for (int i = 1; i < lines.Length; i++)
                    {
                        JObject json = new JObject();
                        for (int j = 0; j < properties.Length; j++)
                        {
                            json.Add(properties[j], csv[i][j]);
                        }
                        jsonData.Add(json);
                    }
                }
            }

            // Json Data Array
            for (int i = 0; i < jsonData.Count; i++)
            {
                jsonAllData += jsonData[i].ToString();
                if (i != jsonData.Count - 1) 
                    jsonAllData += ",";
            }
            jsonAllData += "]";

            return jsonAllData;
        }


        //public static async UniTask Load()
        //{
        //	await AssetDataManager.LoadVersions();
        //	await AssetDataManager.Download();
        //
        //	AssetDataManager.IsLoaded = false;
        //	AssetDataManager.AssetMap.Clear();
        //
        //	await UniTask.WhenAll(new List<UniTask>()
        //		{
        //			AssetDataManager.Load<int, GalaxyRaceLeagueData>("galaxy_race_league"),
        //		});
        //	AssetDataManager.IsLoaded = true;
        //}
    }
}