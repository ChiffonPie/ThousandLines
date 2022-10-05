using Newtonsoft.Json;

namespace ThousandLines_Data
{
    public class MachineAbilityData : AssetData<int>, ISort
    {
        [JsonProperty("stage_prefab")]
        public string StagePrefab { get; private set; }

        [JsonProperty("order")]
        public int OrderIndex { get; private set; }

        [JsonProperty("icon")]
        public string Icon { get; private set; }

    }
}