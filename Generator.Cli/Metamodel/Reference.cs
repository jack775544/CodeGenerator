using System;

namespace Generator.Cli.Metamodel
{
	public abstract record Reference : INamedNode
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string OppositeName { get; set; }
		public Guid SourceId { get; set; }
		public virtual Entity Source { get; set; }
		public Guid TargetId { get; set; }
		public virtual Entity Target { get; set; }
	}

	public record ReferenceOneToOne : Reference
	{
	}

	public record ReferenceOneToMany : Reference
	{
	}

	public record ReferenceManyToMany : Reference
	{
	}
}