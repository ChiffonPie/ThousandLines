namespace ThousandLines_Data
{
	public interface IAssetData
	{
		void Deserialize(Newtonsoft.Json.Linq.JObject data);
	}
}