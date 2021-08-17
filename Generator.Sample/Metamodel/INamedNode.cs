using Generator.Core.Metamodel;

namespace Generator.Sample.Metamodel
{
	public interface INamedNode : IMetamodelNode
	{
		public string Name { get; set; }
	}
}