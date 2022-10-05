using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class LineData : AssetData<int>, ISort
	{
		[JsonProperty("Line_Price")]
		public double Line_Price { get; protected set; }

		[JsonProperty("Line_Speed")]
		public float Line_Speed { get; protected set; }

		[JsonProperty("Line_Order_index")]
		public int OrderIndex { get; private set; }
	}
}