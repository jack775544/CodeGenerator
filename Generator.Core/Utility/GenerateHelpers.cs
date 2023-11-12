using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Generator.Core.Hooks;
using Generator.Core.Templates;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core.Utility
{
	internal static class GenerateHelpers
	{
		private static MethodInfo _generateMethod = typeof(GenerateHelpers)
			.GetMethod("Generate", BindingFlags.Static | BindingFlags.NonPublic)!;

		private static MethodInfo _validateMethod = typeof(GenerateHelpers)
			.GetMethod("Validate", BindingFlags.Static | BindingFlags.NonPublic)!;

		internal static IEnumerable<GenerationResult> Generate<T>(
			ITemplate<T> template,
			IEnumerable<IGenerateHook> hooks,
			IServiceProvider serviceProvider)
		{
			template.ServiceProvider = serviceProvider;
			var generateHooks = hooks.ToList();
			foreach (var entity in template.MapObjects())
			{
				template.Model = entity;
				if (template.Guard())
				{
					template.TransformText();
					foreach (var hook in generateHooks)
					{
						hook.AfterGenerate(template);
					}
					yield return new GenerationResult(
						template.OutputPath,
						template.GetGenerationEnvironment().ToString(),
						template.GetErrors());
					template.ResetGenerationEnvironment();
				}
			}
		}

		internal static IEnumerable<ValidationResult> Validate<T>(IValidationRule<T> rule)
		{
			foreach (var entity in rule.MapObjects())
			{
				rule.Model = entity;
				yield return rule.Validate();
			}
		}

		internal static IEnumerable<Type> GetTemplateTypes(Assembly generatingAssembly)
		{
			return generatingAssembly
				.GetTypes()
				.Where(x => !x.IsAbstract && !x.IsInterface)
				.Where(x => x.GetInterfaces()
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITemplate<>)));
		}
		
		internal static IEnumerable<Type> GetValidationTypes(Assembly generatingAssembly)
		{
			return generatingAssembly
				.GetTypes()
				.Where(x => !x.IsAbstract && !x.IsInterface)
				.Where(x => x.GetInterfaces()
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidationRule<>)));
		}

		internal static IEnumerable<GenerationResult> InvokeTemplate(
			IServiceProvider serviceProvider,
			Type type,
			IEnumerable<IGenerateHook> hooks)
		{
			var template = ActivatorUtilities.CreateInstance(serviceProvider, type);
			var genericArg = type
				.GetInterfaces()
				.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITemplate<>))
				.GetGenericArguments()
				.First();
			var methodInfo = _generateMethod.MakeGenericMethod(genericArg);
			return (IEnumerable<GenerationResult>) methodInfo.Invoke(null, new[] {template, hooks, serviceProvider});
		}

		internal static IEnumerable<ValidationResult> InvokeValidation(IServiceProvider serviceProvider, Type type)
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