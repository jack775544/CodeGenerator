using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Generator.Cli.Metamodel
{
	public record Entity : INamedNode
	{
		[JsonProperty(PropertyName = "$id")]
		public Guid Id { get; set; }
		public List<EntityAttribute> Attributes { get; set; }
		public string Name { get; set; }
	}
}