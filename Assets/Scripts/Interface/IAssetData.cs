namespace ThousandLines
{
	public interface IAssetData
	{
		void Deserialize(Newtonsoft.Json.Linq.JObject data);
	}
}