using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core
{
	internal static class GenerateHelpers
	{
		private static MethodInfo _generateMethod = typeof(GenerateHelpers)
			.GetMethod("Generate", BindingFlags.Static | BindingFlags.Public)!;

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

		public static IEnumerable<Type> GetTemplateTypes(Assembly generatingAssembly)
		{
			return generatingAssembly
				.GetTypes()
				.Where(x => x.GetInterfaces()
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITemplate<>)));
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
	}
}