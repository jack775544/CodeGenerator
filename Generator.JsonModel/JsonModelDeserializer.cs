using System;
using System.Collections.Generic;
using Generator.Core.Metamodel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Generator.JsonModel
{
	public static class JsonModelDeserializer
	{
		public static JsonModelDeserializerResult<T> Deserialize<T>(string json)
			where T : BaseModel
		{
			var nodes = new List<IMetamodelNode>();
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				PreserveReferencesHandling = PreserveReferencesHandling.All,
				MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
				ReferenceResolverProvider = () => new IdReferenceResolver(nodes),
			};
			var model = JsonConvert.DeserializeObject<T>(json, settings);
			return new JsonModelDeserializerResult<T>(model, nodes);
		}
	}
	
	public class IdReferenceResolver : IReferenceResolver
	{
		private readonly List<IMetamodelNode> _allNodes;
		private readonly IDictionary<Guid, IMetamodelNode> _nodes = new Dictionary<Guid, IMetamodelNode>();

		public IdReferenceResolver(List<IMetamodelNode> allNodes)
		{
			_allNodes = allNodes;
		}

		public object ResolveReference(object context, string reference)
		{
			Guid id = new Guid(reference);

			_nodes.TryGetValue(id, out var p);

			return p;
		}

		public string GetReference(object context, object value)
		{
			if (value is not IMetamodelNode metamodelNode)
			{
				return null;
			}

			_nodes[metamodelNode.Id] = metamodelNode;
			return metamodelNode.Id.ToString();

		}

		public bool IsReferenced(object context, object value)
		{
			if (value is not IMetamodelNode metamodelNode)
			{
				return false;
			}

			return _nodes.ContainsKey(metamodelNode.Id);
		}

		public void AddReference(object context, string reference, object value)
		{
			if (value is IMetamodelNode metamodelNode)
			{
				metamodelNode.Id = Guid.Parse(reference);
				var id = new Guid(reference);
				_nodes[id] = metamodelNode;
				_allNodes.Add(metamodelNode);
			}
		}
	}
}