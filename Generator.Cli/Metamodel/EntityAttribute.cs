using Generator.Core.Metamodel;

namespace Generator.Cli.Metamodel
{
	public class EntityAttribute : MetamodelNode, INamedNode
	{
		public string Name { get; set; }
		public string AttributeType { get; set; }
	}
}