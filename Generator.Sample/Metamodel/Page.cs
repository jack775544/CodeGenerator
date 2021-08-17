using System;

namespace Generator.Sample.Metamodel
{
	public record Page : INamedNode
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}