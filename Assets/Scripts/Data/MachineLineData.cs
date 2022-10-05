using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MachineLineData : AssetData<int>, ISort
	{
		[JsonProperty("Line_isActive")]
		public int Line_isActive { get; protected set; }

		[JsonProperty("Line_Order_Index")]
		public int OrderIndex { get; private set; }

		[JsonProperty("Line_Price")]
		public double Line_Price { get; private set; }

		[JsonProperty("Line_Speed")]
		public float Line_Speed { get; private set; }


	}
}