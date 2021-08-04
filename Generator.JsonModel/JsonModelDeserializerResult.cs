using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.JsonModel
{
	public class JsonModelDeserializerResult<T> where T : BaseModel
	{
		public T Model { get; init; }
		public IEnumerable<IMetamodelNode> Nodes { get; init; }

		public JsonModelDeserializerResult(T model, IEnumerable<IMetamodelNode> nodes)
		{
			Model = model;
			Nodes = nodes;
		}
	}
}