using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace AssetData
{
    public class CSVtoJSON
    {
        //public string jsonValue = "Assets/Data";

        public static string ConvertCsvFileToJsonObject(string path)
        {
            List<JObject> jsonData = new List<JObject>();

            string[] files = Directory.GetFiles($"{path}", "*.csv");
            string jsonAllData = null;

            foreach (var file in files)
            {
                UnityEngine.Debug.LogError(Path.GetFileNameWithoutExtension(file));

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

            foreach (var item in jsonData)
            {
                jsonAllData += item.ToString();
            }
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