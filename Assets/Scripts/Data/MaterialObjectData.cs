using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MaterialObjectData : AssetData<int>, ISort
	{
		[JsonProperty("Material_Hp")]
		public int Material_Hp { get; protected set; }

		[JsonProperty("Material_Value")]
		public int Material_Value { get; protected set; }

		[JsonProperty("order")]
		public int OrderIndex { get; protected set; }
	}
}