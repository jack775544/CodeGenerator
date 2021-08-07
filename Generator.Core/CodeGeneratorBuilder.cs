using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generator.Core.Metamodel;
using Generator.Core.Templates;
using Generator.Core.Utility;
using Generator.Core.Validation;
using Generator.Core.Validation.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Generator.Core
{
	public class CodeGeneratorBuilder<TModel> where TModel : BaseModel
	{
		private readonly Assembly _generatingAssembly;
		private readonly IServiceCollection _serviceCollection;
		private readonly List<Type> _templateTypes = new();
		private readonly List<Type> _validationTypes = new();
		private readonly List<Action<ModelBuilder>> _builders = new();
		private readonly Dictionary<Type, List<Type>> _typeMappings = new();

		public CodeGeneratorBuilder(Assembly generatingAssembly)
		{
			_generatingAssembly = generatingAssembly;
			_serviceCollection = new ServiceCollection();

			// Add the node repository
			_serviceCollection.AddScoped<DatabaseIdentifier>();
			_serviceCollection.AddScoped(_ => _builders);
			_serviceCollection.AddDbContext<NodeRepository>((sp, options) =>
			{
				var databaseIdentifier = sp.GetRequiredService<DatabaseIdentifier>();
				options.UseInMemoryDatabase(databaseIdentifier.DatabaseId);
				options.UseLazyLoadingProxies();
			});

			// Register top level model object
			AddMetamodelType<TModel>();
			_serviceCollection.AddScoped(sp => sp.GetRequiredService<NodeRepository>().Set<TModel>().First());
			_serviceCollection.AddScoped<BaseModel>(sp => sp.GetRequiredService<TModel>());

			// Add built in validators
			AddValidatorType<UniqueIdValidator, BaseModel>();
		}

		public CodeGeneratorBuilder<TModel> AddMetamodelType<T>(Action<EntityTypeBuilder<T>> builder = null) where T : class
		{
			_builders.Add(modelBuilder =>
			{
				var entityBuilder = modelBuilder.Entity<T>();
				builder?.Invoke(entityBuilder);
			});

			var nodeType = typeof(T);
			_typeMappings[nodeType] = GetBaseTypes(nodeType).ToList();

			_serviceCollection.AddScoped<IEnumerable<T>>(sp => sp.GetRequiredService<NodeRepository>().Set<T>());

			foreach (var type in GetBaseTypes(nodeType))
			{
				_serviceCollection.TryAdd(new ServiceDescriptor(
					typeof(IEnumerable<>).MakeGenericType(type),
					sp =>
					{
						var nodeList = sp.GetRequiredService<NodeRepository>().GetNodes(type).ToList();
						var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
						foreach (var node in nodeList)
						{
							list.Add(node);
						}

						return list;
					},
					ServiceLifetime.Scoped));
			}

			return this;
		}

		public CodeGeneratorBuilder<TModel> AddMetamodelTypeFactory<T>(
			Func<IServiceProvider, IEnumerable<T>> builder)
			where T : class
		{
			_serviceCollection.AddScoped(builder);
			return this;
		}

		public CodeGeneratorBuilder<TModel> AddTemplateType<TTemplate, TEntity>()
			where TTemplate : ITemplate<TEntity>
		{
			_templateTypes.Add(typeof(TTemplate));
			return this;
		}

		public CodeGeneratorBuilder<TModel> AutoWireTemplateTypes(Assembly assembly = null)
		{
			_templateTypes.AddRange(GenerateHelpers.GetTemplateTypes(assembly ?? _generatingAssembly));
			return this;
		}

		public CodeGeneratorBuilder<TModel> AddValidatorType<TRule, TEntity>()
			where TRule : IValidationRule<TEntity>
			where TEntity : IMetamodelNode
		{
			_validationTypes.Add(typeof(TRule));
			return this;
		}
		
		public CodeGeneratorBuilder<TModel> AutoWireValidationTypes(Assembly assembly = null)
		{
			_validationTypes.AddRange(GenerateHelpers.GetValidationTypes(assembly ?? _generatingAssembly));
			return this;
		}

		public CodeGenerator<TModel> Build()
		{
			return new(
				_serviceCollection.BuildServiceProvider(),
				_templateTypes,
				_validationTypes,
				_typeMappings);
		}

		private static IEnumerable<Type> GetBaseTypes(Type type)
		{
			var types = new List<Type>();
			types.AddRange(type.GetInterfaces());

			var t = type;
			while (t.BaseType != null)
			{
				types.Add(t.BaseType);
				t = t.BaseType;
			}

			return types;
		}
	}
}