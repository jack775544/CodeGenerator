using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Core.Metamodel;
using Microsoft.EntityFrameworkCore;

namespace Generator.Core.Utility
{
	public class NodeRepository : DbContext
	{
		private readonly List<Action<ModelBuilder>> _builders;
		private readonly IServiceProvider _serviceProvider;

		public NodeRepository(
			DbContextOptions<NodeRepository> options,
			List<Action<ModelBuilder>> builders,
			IServiceProvider serviceProvider) :
			base(options)
		{
			_builders = builders;
			_serviceProvider = serviceProvider;
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
			return Model
				.GetEntityTypes()
				.Where(entityType => type.IsAssignableFrom(entityType.ClrType))
				.Where(nodeType => !(nodeType.ClrType.IsAbstract || nodeType.ClrType.IsInterface))
				.SelectMany(nodeType =>
				{
					var serviceType = typeof(IEnumerable<>).MakeGenericType(nodeType.ClrType);
					return (IEnumerable<IMetamodelNode>) _serviceProvider.GetService(serviceType);
				});
		}
	}
}