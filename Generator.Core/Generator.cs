using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core
{
	public class Generator
	{
		private readonly Assembly _generatingAssembly;
		private readonly IServiceCollection _serviceCollection;

		public Generator(Assembly generatingAssembly)
		{
			_generatingAssembly = generatingAssembly;
			_serviceCollection = new ServiceCollection();
		}

		public Generator AddMetaModelType<T>(Func<IServiceProvider, IEnumerable<T>> implementationFactory) where T : class
		{
			_serviceCollection.AddSingleton(implementationFactory);
			return this;
		}

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

		private Type[] GetTemplateTypes()
		{
			return (
					from x in _generatingAssembly.GetTypes()
					from z in x.GetInterfaces()
					let y = x.BaseType
					where y != null && y.IsGenericType
					where !x.IsAbstract && !x.IsInterface
					where typeof(ITemplate<>).IsAssignableFrom(y.GetGenericTypeDefinition()) || z.IsGenericType
						&& typeof(ITemplate<>).IsAssignableFrom(z.GetGenericTypeDefinition())
					select x)
				.ToArray();
		}

		private static MethodInfo _generateMethod =
			typeof(Generator).GetMethod("Generate", BindingFlags.Static | BindingFlags.NonPublic)!;

		private IEnumerable<GenerationResult> InvokeTemplate(IServiceProvider serviceProvider, Type type)
		{
			var template = ActivatorUtilities.CreateInstance(serviceProvider, type);
			var methodInfo =
				_generateMethod.MakeGenericMethod(type.GetInterface("ITemplate`1").GetGenericArguments().First());
			return (IEnumerable<GenerationResult>) methodInfo.Invoke(null, new[] {template});
		}
	}
}