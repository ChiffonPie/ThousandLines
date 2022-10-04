using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssetData
{
	public abstract class AssetData<T> : IAssetData
	{
		[JsonProperty("Id")]
		public virtual T Id { get; protected set; }

		public virtual void Deserialize(JObject data) { }
	}
}