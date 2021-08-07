using System;
using System.Collections.Generic;

namespace Generator.Cli.Metamodel
{
	public record Entity : INamedNode
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public virtual ICollection<EntityAttribute> Attributes { get; set; }

		// public virtual ICollection<Reference> IncomingReferences { get; set; }
		//
		// public virtual ICollection<Reference> OutgoingReferences { get; set; }
		
		public virtual ICollection<ReferenceOneToMany> IncomingOneToManyReferences { get; set; }

		public virtual ICollection<ReferenceOneToMany> OutgoingOneToManyReferences { get; set; }
		
		public virtual ICollection<ReferenceOneToOne> IncomingOneToOneReferences { get; set; }

		public virtual ICollection<ReferenceOneToOne> OutgoingOneToOneReferences { get; set; }
		
		public virtual ICollection<ReferenceManyToMany> IncomingManyToManyReferences { get; set; }

		public virtual ICollection<ReferenceManyToMany> OutgoingManyToManyReferences { get; set; }
	}
}