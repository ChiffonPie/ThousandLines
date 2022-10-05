using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MachineLineData : AssetData<int>, ISort
	{
		[JsonProperty("Machine_isActive")]
		public int Machine_isActive { get; protected set; }

		[JsonProperty("Machine_Create_Speed")]
		public float Machine_Create_Speed { get; protected set; }

		[JsonProperty("Machine_Speed")]
		public float Machine_Speed { get; private set; }

		[JsonProperty("order")]
		public int OrderIndex { get; private set; }
	}
}