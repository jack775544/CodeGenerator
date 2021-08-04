using System;
using System.Collections.Generic;
using Generator.Core.Metamodel;
using Newtonsoft.Json;

namespace Generator.JsonModel
{
	public class JsonModelDeserializer
	{
		public static JsonModelDeserializerResult<T> Deserialize<T>(string json)
			where T : BaseModel
		{
			var nodes = new List<IMetamodelNode>();
			var model = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
			{
				Converters = new List<JsonConverter>
				{
					new NodeConverter(nodes),
				},
			});
			return new JsonModelDeserializerResult<T>(model, nodes);
		}
	}

	public class NodeConverter : JsonConverter
	{
		private readonly List<IMetamodelNode> _nodes;

		public NodeConverter(List<IMetamodelNode> nodes)
		{
			_nodes = nodes;
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			throw new NotImplementedException("Can currently only read");
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			var instance = Activator.CreateInstance(objectType);
			serializer.Populate(reader, instance);
			_nodes.Add((IMetamodelNode)instance);
			return instance;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.IsAssignableTo(typeof(IMetamodelNode));
		}
	}
}