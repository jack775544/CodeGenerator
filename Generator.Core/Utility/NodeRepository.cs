using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generator.Core.Metamodel;
using Microsoft.EntityFrameworkCore;

namespace Generator.Core.Utility
{
	public class NodeRepository : DbContext
	{
		private MethodInfo _setMethod = typeof(NodeRepository)
			.GetMethods()
			.First(x => x.Name == "Set" && x.GetParameters().Length == 0);

		private readonly List<Action<ModelBuilder>> _builders;

		public NodeRepository(DbContextOptions<NodeRepository> options, List<Action<ModelBuilder>> builders) :
			base(options)
		{
			_builders = builders;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var builder in _builders)
			{
				builder(modelBuilder);
			}
		}

		public IEnumerable<IMetamodelNode> GetNodes(Type type)
		{
			var entityTypes = Model.GetEntityTypes().ToList();
			var validTypes = entityTypes
				.Where(entityType => type.IsAssignableFrom(entityType.ClrType))
				.ToList();
			return validTypes
				.Select(entityType => (IQueryable<IMetamodelNode>) _setMethod
					.MakeGenericMethod(entityType.ClrType)
					.Invoke(this, Array.Empty<object>()))
				.SelectMany(x => x)
				.ToHashSet();
		}
	}
}