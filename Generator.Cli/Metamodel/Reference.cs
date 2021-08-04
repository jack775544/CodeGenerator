using System;

namespace Generator.Cli.Metamodel
{
	public class Reference : INamedNode
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string OppositeName { get; set; }
		public Entity Source { get; set; }
		public Entity Target { get; set; }
	}
}