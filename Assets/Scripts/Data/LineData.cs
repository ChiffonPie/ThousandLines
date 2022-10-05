using Newtonsoft.Json;

namespace ThousandLines
{
	public class LineData : AssetData<int>, ISort
	{
		[JsonProperty("Line_isActive")]
		public int Line_isActive { get; protected set; }

		[JsonProperty("Line_Price")]
		public double Line_Price { get; protected set; }

		[JsonProperty("Line_Speed")]
		public float Line_Speed { get; protected set; }

		[JsonProperty("Line_Order_index")]
		public int OrderIndex { get; private set; }

		[JsonIgnore]
		public bool IsActive
		{
			get
			{
				if (this.Line_isActive == 0) return false;
				else return true;
			}
		}
	}
}