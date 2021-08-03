using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Generator.Core.Metamodel;
using Generator.Core.Templates;
using Generator.Core.Utility;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Generator.Core
{
	public class CodeGenerator<TModel> where TModel : class
	{
		private readonly Assembly _generatingAssembly;
		public readonly IServiceCollection _serviceCollection;
		private readonly List<Type> _templateTypes = new();
		private readonly List<Type> _validationTypes = new();
		private readonly Dictionary<Type, List<Func<IServiceProvider, IEnumerable<object>>>> _typeLookup = new();

		public CodeGenerator(TModel model, Assembly generatingAssembly)
		{
			_generatingAssembly = generatingAssembly;
			_serviceCollection = new ServiceCollection();
			_serviceCollection.AddSingleton(_ => model);
		}

		public CodeGenerator<TModel> AddMetaModelType<T>(
			Func<IServiceProvider, IEnumerable<T>> implementationFactory)
			where T : MetamodelNode
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
			where TEntity : MetamodelNode
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