using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Core.Hooks;
using Generator.Core.Metamodel;
using Generator.Core.Templates;
using Generator.Core.Utility;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core
{
	public class CodeGeneratorScope<T> : IServiceScope
	{
		private readonly IServiceScope _scope;
		private readonly List<Type> _templateTypes;
		private readonly List<Type> _validationTypes;
		private readonly List<IGenerateHook> _hooks;

		public IServiceProvider ServiceProvider => _scope.ServiceProvider;

		internal CodeGeneratorScope(IServiceScope scope, List<Type> templateTypes, List<Type> validationTypes, List<IGenerateHook> hooks)
		{
			_scope = scope;
			_templateTypes = templateTypes;
			_validationTypes = validationTypes;
			_hooks = hooks;
		}
		
		public IEnumerable<GenerationResult> GenerateAll()
		{
			return _templateTypes
				.Select(x => GenerateHelpers.InvokeTemplate(ServiceProvider, x, _hooks))
				.SelectMany(x => x);
		}

		public IEnumerable<ValidationResult> ValidateAll()
		{
			return _validationTypes
				.Select(x => GenerateHelpers.InvokeValidation(ServiceProvider, x))
				.SelectMany(x => x);
		}
		
		public IEnumerable<GenerationResult> Generate<TModel>(ITemplate<TModel> template)
		{
			return GenerateHelpers.Generate(template, _hooks);
		}

		public IEnumerable<ValidationResult> Validate<TModel>(IValidationRule<TModel> rule) where TModel : IMetamodelNode
		{
			return GenerateHelpers.Validate(rule);
		}

		public void Dispose()
		{
			_scope.Dispose();
		}
	}
}