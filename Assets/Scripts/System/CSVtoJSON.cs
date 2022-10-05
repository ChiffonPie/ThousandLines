using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace ThousandLines
{
    public class CSVtoJSON
    {
        //특정 파일을 파싱하는 방식
        public static string ConvertCsvFileToJson(string fileName)
        {
            List<JObject> jsonData = new List<JObject>();
            string[] files = Directory.GetFiles($"{AssetDataManager.dataPath}", "*.csv");
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

        #region Others
        /*
           // 모든 데이터를 전체를 파싱하는 방식 (서버 패킷 구조와 동일하도록)
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
               return jsonAllData;
           }
        */
        #endregion
    }
}