using System;
using System.Collections.Generic;
using Generator.Core;
using Generator.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Generator.JsonModel;

internal class ModelBuilderConfigurations : List<Action<ModelBuilder>> {}

public static class JsonCodeGeneratorBuilderExtensions
{
	public static CodeGeneratorBuilder AddMetamodelType<T>(
		this CodeGeneratorBuilder self,
		Action<EntityTypeBuilder<T>> builder = null) where T : class
	{
		self.Configure<ModelBuilderConfigurations>(options => options.Add(modelBuilder =>
		{
			var entityBuilder = modelBuilder.Entity<T>();
			builder?.Invoke(entityBuilder);
		}));

		var nodeType = typeof(T);

		self.TryAddScoped<IEnumerable<T>>(sp => sp.GetRequiredService<JsonNodeRepository>().Set<T>());

		foreach (var type in TypeHelpers.GetBaseTypes(nodeType))
		{
			self.TryAddScoped(
				typeof(IEnumerable<>).MakeGenericType(type),
				sp => sp.GetRequiredService<JsonNodeRepository>().GetNodes(type));
				
			// self.TryAdd(new ServiceDescriptor(
			// 	typeof(IEnumerable<>).MakeGenericType(type),
			// 	sp =>
			// 	{
			// 		var nodeList = sp.GetRequiredService<NodeRepository>().GetNodes(type).ToList();
			// 		var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
			// 		foreach (var node in nodeList)
			// 		{
			// 			list.Add(node);
			// 		}
			//
			// 		return list;
			// 	},
			// 	ServiceLifetime.Scoped));
		}

		return self;
	}
}