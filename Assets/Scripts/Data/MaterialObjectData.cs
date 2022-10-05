using Newtonsoft.Json;

namespace ThousandLines_Data
{
	public class MaterialObjectData : AssetData<int>, ISort
	{
		[JsonProperty("Material_Hp")]
		public int Material_Hp { get; private set; }

		[JsonProperty("Material_Value")]
		public int Material_Value { get; private set; }

		[JsonProperty("order")]
		public int OrderIndex { get; private set; }
	}
}