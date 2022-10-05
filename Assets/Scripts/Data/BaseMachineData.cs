using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class BaseMachineData : AssetData<int>
	{
		[JsonProperty("Machine_Create_Speed")]
		public float Machine_Create_Speed { get; protected set; }

		[JsonProperty("Machine_Speed")]
		public float Machine_Speed { get; private set; }
	}
}