using System;
using System.Collections.Generic;
using System.Reflection;
using Generator.Core.Hooks;
using Generator.Core.Templates;
using Generator.Core.Utility;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core;

public class TemplateTypeCollection : List<Type> {}
public class ValidationTypeCollection : List<Type> {}
public class GenerateHookCollection : List<IGenerateHook> {}

public class CodeGeneratorBuilder : ServiceCollection
{
	private readonly Assembly _generatingAssembly;

	// private readonly IServiceCollection _serviceCollection;
	private readonly TemplateTypeCollection _templateTypes = new();

	private readonly ValidationTypeCollection _validationTypes = new();

	// private readonly List<Action<ModelBuilder>> _builders = new();
	private readonly GenerateHookCollection _hooks = new();

	public CodeGeneratorBuilder(Assembly generatingAssembly)
	{
		_generatingAssembly = generatingAssembly;
		// _serviceCollection = new ServiceCollection();

		// Add the node repository
		// _serviceCollection.AddScoped<DatabaseIdentifier>();
		// _serviceCollection.AddScoped(_ => _builders);
		// _serviceCollection.AddDbContext<NodeRepository>((sp, options) =>
		// {
		// 	var databaseIdentifier = sp.GetRequiredService<DatabaseIdentifier>();
		// 	options.UseInMemoryDatabase(databaseIdentifier.DatabaseId);
		// 	options.UseLazyLoadingProxies();
		// });

		// Register top level model object
		// AddMetamodelType<TModel>();
		// _serviceCollection.AddScoped(sp => sp.GetRequiredService<NodeRepository>().Set<TModel>().First());
		// _serviceCollection.AddScoped<BaseModel>(sp => sp.GetRequiredService<TModel>());

		this.AddSingleton(_templateTypes);
		this.AddSingleton(_validationTypes);
		this.AddSingleton(_hooks);

		// Add built in validators
		// AddValidatorType<UniqueIdValidator, BaseModel>();
	}

	// public CodeGeneratorBuilder RegisterEPackage(EPackage ePackage)
	// {
	// 	var types = ePackage
	// 		.getEClassifiers()
	// 		.GetEnumerableOfType<EClass>()
	// 		.Select(x => x.getInstanceClassName())
	// 		.Select(x => ePackage.GetType().Assembly.GetType(x)!)
	// 		// .Select(x => new ServiceDescriptor())
	// 		.ToList();
	//
	// 	foreach (var type in types)
	// 	{
	// 		AddResource(type);
	// 		foreach (var baseType in GetBaseTypes(type))
	// 		{
	// 			AddResource(baseType);
	// 		}
	// 	}
	//
	// 	return this;
	// }
	//
	// private void AddResource(Type type)
	// {
	// 	_serviceCollection.TryAddScoped(
	// 		typeof(IEnumerable<>).MakeGenericType(type),
	// 		sp => sp.GetRequiredService<NodeRepository>().GetNodes(type));
	// }

	// private void RegisterBaseTypes(Type nodeType)
	// {
	// 	foreach (var type in GetBaseTypes(nodeType))
	// 	{
	// 		_serviceCollection.TryAdd(new ServiceDescriptor(
	// 			typeof(IEnumerable<>).MakeGenericType(type),
	// 			sp =>
	// 			{
	// 				var nodeList = sp.GetRequiredService<NodeRepository>().GetNodes(type).ToList();
	// 				var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
	// 				foreach (var node in nodeList)
	// 				{
	// 					list.Add(node);
	// 				}
	//
	// 				return list;
	// 			},
	// 			ServiceLifetime.Scoped));
	// 	}
	// }
	//
	// public CodeGeneratorBuilder<TModel> AddMetamodelType<T>(Action<EntityTypeBuilder<T>> builder = null) where T : class
	// {
	// 	_builders.Add(modelBuilder =>
	// 	{
	// 		var entityBuilder = modelBuilder.Entity<T>();
	// 		builder?.Invoke(entityBuilder);
	// 	});
	//
	// 	var nodeType = typeof(T);
	//
	// 	_serviceCollection.AddScoped<IEnumerable<T>>(sp => sp.GetRequiredService<NodeRepository>().Set<T>());
	//
	// 	foreach (var type in GetBaseTypes(nodeType))
	// 	{
	// 		_serviceCollection.TryAdd(new ServiceDescriptor(
	// 			typeof(IEnumerable<>).MakeGenericType(type),
	// 			sp =>
	// 			{
	// 				var nodeList = sp.GetRequiredService<NodeRepository>().GetNodes(type).ToList();
	// 				var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
	// 				foreach (var node in nodeList)
	// 				{
	// 					list.Add(node);
	// 				}
	//
	// 				return list;
	// 			},
	// 			ServiceLifetime.Scoped));
	// 	}
	//
	// 	return this;
	// }

	// public CodeGeneratorBuilder<TModel> AddMetamodelTypeFactory<T>(
	// 	Func<IServiceProvider, IEnumerable<T>> builder)
	// 	where T : class
	// {
	// 	_serviceCollection.AddScoped(builder);
	// 	return this;
	// }

	public CodeGeneratorBuilder AddTemplateType<TTemplate, TEntity>()
		where TTemplate : ITemplate<TEntity>
	{
		_templateTypes.Add(typeof(TTemplate));
		return this;
	}

	public CodeGeneratorBuilder AutoWireTemplateTypes(Assembly assembly = null)
	{
		_templateTypes.AddRange(GenerateHelpers.GetTemplateTypes(assembly ?? _generatingAssembly));
		return this;
	}

	public CodeGeneratorBuilder AddValidatorType<TRule, TEntity>()
		where TRule : IValidationRule<TEntity>
	{
		_validationTypes.Add(typeof(TRule));
		return this;
	}

	public CodeGeneratorBuilder AutoWireValidationTypes(Assembly assembly = null)
	{
		_validationTypes.AddRange(GenerateHelpers.GetValidationTypes(assembly ?? _generatingAssembly));
		return this;
	}

	public CodeGeneratorBuilder AddGenerationHook(IGenerateHook hook)
	{
		_hooks.Add(hook);
		return this;
	}

	// public CodeGeneratorBuilder AddSingletonHelper<T>() where T : class
	// {
	// 	this.TryAddSingleton<T>();
	// 	return this;
	// }
	//
	// public CodeGeneratorBuilder AddScopedHelper<T>() where T : class
	// {
	// 	this.TryAddScoped<T>();
	// 	return this;
	// }

	// public CodeGenerator Build()
	// {
	// 	return new(
	// 		this.BuildServiceProvider(),
	// 		_templateTypes,
	// 		_validationTypes,
	// 		_hooks);
	// }
}