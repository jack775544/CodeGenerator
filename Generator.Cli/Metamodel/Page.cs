using Generator.Core.Metamodel;

namespace Generator.Cli.Metamodel
{
	public class Page : MetamodelNode, INamedNode
	{
		public string Name { get; set; }
	}
}