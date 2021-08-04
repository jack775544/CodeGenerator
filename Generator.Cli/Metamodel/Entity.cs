using System;
using System.Collections.Generic;

namespace Generator.Cli.Metamodel
{
	public record Entity : INamedNode
	{
		public Guid Id { get; set; }
		public List<EntityAttribute> Attributes { get; set; }
		public string Name { get; set; }
	}
}