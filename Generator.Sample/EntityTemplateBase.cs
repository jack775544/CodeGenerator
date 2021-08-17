using System.Collections.Generic;
using Generator.Core.Templates;
using Generator.Sample.Metamodel;

namespace Generator.Sample
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