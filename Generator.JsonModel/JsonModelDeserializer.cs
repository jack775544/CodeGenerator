using Generator.Core.Metamodel;
using Newtonsoft.Json;

namespace Generator.JsonModel
{
	public static class JsonModelDeserializer
	{
		public static T Deserialize<T>(string json)
			where T : BaseModel
		{
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
			};
			return JsonConvert.DeserializeObject<T>(json, settings);
		}
	}
}