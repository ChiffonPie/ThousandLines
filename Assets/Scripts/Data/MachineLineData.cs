using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MachineLineData : AssetData<string>, ISort
	{
		[JsonProperty("Line_isActive")]
		public int Line_isActive { get; protected set; }

		[JsonProperty("Line_isGet")]
		public int Line_isGet { get; protected set; }

		[JsonProperty("Line_Order_Index")]
		public int OrderIndex { get; private set; }

		[JsonProperty("Line_Price")]
		public double Line_Price { get; private set; }

		[JsonProperty("Line_Speed")]
		public float Line_Speed { get; private set; }

		[JsonProperty("Line_Distance")]
		public float Line_Distance { get; private set; }

		[JsonProperty("Line_Prosseing")]
		public string Line_Prosseing { get; private set; }

		[JsonProperty("Line_Description")]
		public string Line_Description { get; private set; }
	}
}