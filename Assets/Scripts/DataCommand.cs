using Newtonsoft.Json;

namespace AssetData
{
    public class DataCommand<T> : AssetData<string>
    {
        [JsonProperty("Count")]
        public int Count { get; protected set; }
    }
}