using System.Collections.Generic;
using Generator.Cli.Metamodel;
using Generator.Core.Templates;

namespace Generator.Cli
{
	public abstract class EntityTemplateBase : AbstractTemplate<Entity>
	{
		private readonly IEnumerable<Entity> _entities;

		public EntityTemplateBase(IEnumerable<Entity> entities)
		{
			_entities = entities;
		}

		public override IEnumerable<Entity> MapObjects()
		{
			return _entities;
		}
	}
}