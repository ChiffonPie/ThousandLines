using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class BaseMachineData : AssetData<int>
	{
		[JsonProperty("Machine_isActive")]
		public int Machine_isActive { get; protected set; }

		[JsonProperty("Machine_Create_Speed")]
		public float Machine_Create_Speed { get; protected set; }

		[JsonProperty("Machine_Speed")]
		public float Machine_Speed { get; protected set; }

		[JsonProperty("Machine_Distance")]
		public float Machine_Distance { get; protected set; }
	}
}