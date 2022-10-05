using Newtonsoft.Json;

namespace ThousandLines
{
    public class DataCommand<T> : AssetData<string>
    {
        [JsonProperty("Count")]
        public int Count { get; protected set; }
    }
}