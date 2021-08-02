using System.Collections.Generic;

namespace Generator.Cli.Metamodel
{
	public class Entity
	{
		public string Name { get; set; }
		public List<EntityAttribute> Attributes { get; set; }
	}
}