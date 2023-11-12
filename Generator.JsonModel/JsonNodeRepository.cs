using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generator.Core.Utility;
using Microsoft.EntityFrameworkCore;

namespace Generator.JsonModel;

public class JsonNodeRepository : DbContext
{
	private static readonly MethodInfo DbContextSet = typeof(DbContext)
		.GetMethods()
		.First(x => x.Name == nameof(Set) && x.GetParameters().Length == 0);

	private readonly ModelBuilderConfigurations _builders;

	internal JsonNodeRepository(
		DbContextOptions<JsonNodeRepository> options,
		ModelBuilderConfigurations builders) : base(options)
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
	
	public object GetNodes(Type type)
	{
		return TypeHelpers.CastRuntime(
			Model
				.GetEntityTypes()
				.Where(entityType => type.IsAssignableFrom(entityType.ClrType))
				.Where(nodeType => !(nodeType.ClrType.IsAbstract || nodeType.ClrType.IsInterface))
				.SelectMany(nodeType => (IEnumerable<object>) DbContextSet
					.MakeGenericMethod(nodeType.ClrType)
					.Invoke(this, Array.Empty<object>())),
			type);
	}
}