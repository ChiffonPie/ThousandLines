using Newtonsoft.Json;

namespace ThousandLines_Data
{
    public class MachineAbilityData : AssetData<string>
    {
        [JsonProperty("MachineAbility_Damage")]
        public int MachineAbility_Damage { get; private set; }

        [JsonProperty("MachineAbility_Plus")]
        public double MachineAbility_Plus { get; private set; }

        [JsonProperty("MachineAbility_Multiply")]
        public int MachineAbility_Multiply { get; private set; }

        [JsonProperty("MachineAbility_Shield")]
        public int MachineAbility_Shield { get; private set; }

    }
}