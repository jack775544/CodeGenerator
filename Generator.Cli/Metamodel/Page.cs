using System;

namespace Generator.Cli.Metamodel
{
	public record Page : INamedNode
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}