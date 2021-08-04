using System;

namespace Generator.Cli.Metamodel
{
	public record EntityAttribute : INamedNode
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string AttributeType { get; set; }
	}
}