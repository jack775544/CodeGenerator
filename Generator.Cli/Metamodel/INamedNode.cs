using Generator.Core.Metamodel;

namespace Generator.Cli.Metamodel
{
	public interface INamedNode : IMetamodelNode
	{
		public string Name { get; set; }
	}
}