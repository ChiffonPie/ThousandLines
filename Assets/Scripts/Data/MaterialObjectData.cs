using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MaterialObjectData : AssetData<string>, ISort
	{
		[JsonProperty("Material_Price")]
		public int Material_Price { get; protected set; }

		[JsonProperty("Material_Value")]
		public int Material_Value { get; protected set; }

		[JsonProperty("order")]
		public int OrderIndex { get; protected set; }
	}
}