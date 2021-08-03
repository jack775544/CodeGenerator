using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Cli.Metamodel
{
	public class Entity : MetamodelNode, INamedNode
	{
		public List<EntityAttribute> Attributes { get; set; }
		public string Name { get; set; }
	}
}