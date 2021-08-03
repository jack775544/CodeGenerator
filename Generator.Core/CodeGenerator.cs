using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generator.Core.Templates;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core
{
	public class CodeGenerator<TModel> where TModel : class
	{
		private readonly Assembly _generatingAssembly;
		private readonly IServiceCollection _serviceCollection;
		private readonly List<Type> _templateTypes = new();
		private readonly List<Type> _validationTypes = new();

		public CodeGenerator(TModel model, Assembly generatingAssembly)
		{
			_generatingAssembly = generatingAssembly;
			_serviceCollection = new ServiceCollection();
			_serviceCollection.AddSingleton(_ => model);
		}

		public CodeGenerator<TModel> AddMetaModelType<T>(
			Func<IServiceProvider, IEnumerable<T>> implementationFactory)
			where T : class
		{
			_serviceCollection.AddSingleton(implementationFactory);
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
	}
}