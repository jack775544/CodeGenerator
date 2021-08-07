using System;
using System.Collections.Generic;

namespace Generator.Cli.Metamodel
{
	public record Entity : INamedNode
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public virtual ICollection<EntityAttribute> Attributes { get; set; }

		public virtual ICollection<Reference> IncomingReferences { get; set; }

		public virtual ICollection<Reference> OutgoingReferences { get; set; }
	}
}