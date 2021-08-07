using System;

namespace Generator.Cli.Metamodel
{
	public abstract record EntityAttribute : INamedNode
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public virtual Entity Entity { get; set; }
	}

	public record AttributeString : EntityAttribute
	{
		public int MaxLength { get; set; }
	}
	
	public record AttributeBoolean : EntityAttribute
	{
	}

	public record AttributeDateTime : EntityAttribute
	{
	}
}