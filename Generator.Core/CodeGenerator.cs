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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Generator.Core
{
	public class CodeGenerator<TModel> where TModel : BaseModel
	{
		private readonly Assembly _generatingAssembly;
		private readonly IServiceCollection _serviceCollection;
		private readonly List<Type> _templateTypes = new();
		private readonly List<Type> _validationTypes = new();
		private readonly List<IMetamodelNode> _nodes;

		public CodeGenerator(TModel model, IEnumerable<IMetamodelNode> nodes, Assembly generatingAssembly)
		{
			_generatingAssembly = generatingAssembly;
			_nodes = nodes.ToList();
			_serviceCollection = new ServiceCollection();
			
			// Register top level model object
			_serviceCollection.AddSingleton(_ => model);
			_serviceCollection.AddSingleton<BaseModel>(_ => model);
			AddMetaModelType(_ => new []{ model });

			// Add built in validators
			AddValidatorType<IdRequiredValidationRule, IMetamodelNode>();
			AddValidatorType<UniqueIdValidator, BaseModel>();
		}

		public CodeGenerator<TModel> AddMetaModelType<T>(
			Func<IServiceProvider, IEnumerable<T>> implementationFactory)
			where T : class, IMetamodelNode
		{
			_serviceCollection.AddSingleton(implementationFactory);
			var metamodelType = typeof(T);
			
			// Register each interface on the node
			foreach (var nodeInterface in GetBaseTypes(metamodelType))
			{
				_serviceCollection.Configure<ServiceFactory>(nodeInterface.AssemblyQualifiedName, f =>
				{
					f.AddFactory(implementationFactory);
				});
				var generic = typeof(IEnumerable<>).MakeGenericType(nodeInterface);
				_serviceCollection.TryAdd(new ServiceDescriptor(
					generic,
					sp =>
					{
						var items = sp
							.GetRequiredService<IOptionsSnapshot<ServiceFactory>>()
							.Get(nodeInterface.AssemblyQualifiedName)
							.Invoke(sp)
							.SelectMany(x => x);
						var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(nodeInterface));
						foreach (var item in items)
						{
							list.Add(item);
						}
						return list;
					},
					ServiceLifetime.Singleton));
			}
			return this;
		}

		public CodeGenerator<TModel> AddTemplateType<TTemplate, TEntity>()
			where TTemplate : ITemplate<TEntity>
		{
			_templateTypes.Add(typeof(TTemplate));
			return this;
		}
		
		public CodeGenerator<TModel> AutoWireTemplateTypes()
		{
			_templateTypes.AddRange(GenerateHelpers.GetTemplateTypes(_generatingAssembly));
			return this;
		}
		
		public CodeGenerator<TModel> AddValidatorType<TRule, TEntity>()
			where TRule : IValidationRule<TEntity>
			where TEntity : IMetamodelNode
		{
			_validationTypes.Add(typeof(TRule));
			return this;
		}
		
		public CodeGenerator<TModel> AutoWireValidationTypes()
		{
			_validationTypes.AddRange(GenerateHelpers.GetValidationTypes(_generatingAssembly));
			return this;
		}

		public IEnumerable<GenerationResult> GenerateAll()
		{
			var serviceProvider = _serviceCollection.BuildServiceProvider();
			return _templateTypes
				.Select(x => GenerateHelpers.InvokeTemplate(serviceProvider, x))
				.SelectMany(x => x);
		}

		public IEnumerable<ValidationResult> ValidateAll()
		{
			var serviceProvider = _serviceCollection.BuildServiceProvider();
			return _validationTypes
				.Select(x => GenerateHelpers.InvokeValidation(serviceProvider, x))
				.SelectMany(x => x);
		}
		
		public static IEnumerable<GenerationResult> Generate<T>(ITemplate<T> template)
		{
			return GenerateHelpers.Generate(template);
		}
		
		public static IEnumerable<ValidationResult> Validate<T>(IValidationRule<T> rule) where T : IMetamodelNode
		{
			return GenerateHelpers.Validate(rule);
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