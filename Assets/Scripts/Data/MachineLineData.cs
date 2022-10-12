using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MachineLineData : AssetData<string>, ISort
	{
		[JsonProperty("Line_Order_Index")]
		public int OrderIndex { get; protected set; }

		[JsonProperty("Line_Setting_Index")]
		public int Line_Setting_Index { get; protected set; }

		[JsonProperty("Line_isActive")]
		public int Line_isActive { get; protected set; }

		[JsonProperty("Line_isGet")]
		public int Line_isGet { get; protected set; }

		[JsonProperty("Line_Price")]
		public double Line_Price { get; protected set; }

		[JsonProperty("Line_Speed")]
		public float Line_Speed { get; protected set; }

		[JsonProperty("Line_Distance")]
		public float Line_Distance { get; protected set; }

		[JsonProperty("Line_Prosseing")]
		public string Line_Prosseing { get; protected set; }

		[JsonProperty("Line_Description")]
		public string Line_Description { get; protected set; }
	}
}