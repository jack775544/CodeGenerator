using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generator.Core.Metamodel;
using Generator.Core.Templates;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core.Utility
{
	internal static class GenerateHelpers
	{
		private static MethodInfo _generateMethod = typeof(GenerateHelpers)
			.GetMethod("Generate", BindingFlags.Static | BindingFlags.Public)!;

		private static MethodInfo _validateMethod = typeof(GenerateHelpers)
			.GetMethod("Validate", BindingFlags.Static | BindingFlags.Public)!;

		public static IEnumerable<GenerationResult> Generate<T>(ITemplate<T> template)
		{
			foreach (var entity in template.MapObjects())
			{
				if (template.Guard())
				{
					template.Model = entity;
					yield return new GenerationResult(template.OutputPath, template.TransformText());
					template.ResetGenerationEnvironment();
				}
			}
		}

		public static IEnumerable<ValidationResult> Validate<T>(IValidationRule<T> rule) where T : IMetamodelNode
		{
			foreach (var entity in rule.MapObjects())
			{
				rule.Model = entity;
				yield return rule.Validate();
			}
		}

		public static IEnumerable<Type> GetTemplateTypes(Assembly generatingAssembly)
		{
			return generatingAssembly
				.GetTypes()
				.Where(x => !x.IsAbstract && !x.IsInterface)
				.Where(x => x.GetInterfaces()
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITemplate<>)));
		}
		
		public static IEnumerable<Type> GetValidationTypes(Assembly generatingAssembly)
		{
			return generatingAssembly
				.GetTypes()
				.Where(x => !x.IsAbstract && !x.IsInterface)
				.Where(x => x.GetInterfaces()
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidationRule<>)));
		}

		public static IEnumerable<GenerationResult> InvokeTemplate(IServiceProvider serviceProvider, Type type)
		{
			var template = ActivatorUtilities.CreateInstance(serviceProvider, type);
			var genericArg = type
				.GetInterfaces()
				.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITemplate<>))
				.GetGenericArguments()
				.First();
			var methodInfo = _generateMethod.MakeGenericMethod(genericArg);
			return (IEnumerable<GenerationResult>) methodInfo.Invoke(null, new[] {template});
		}

		public static IEnumerable<ValidationResult> InvokeValidation(IServiceProvider serviceProvider, Type type)
		{
			var rule = ActivatorUtilities.CreateInstance(serviceProvider, type);
			var genericArg = type
				.GetInterfaces()
				.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidationRule<>))
				.GetGenericArguments()
				.First();
			var methodInfo = _validateMethod.MakeGenericMethod(genericArg);
			return (IEnumerable<ValidationResult>) methodInfo.Invoke(null, new[] {rule});
		}
	}
}