using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Generator.Cli.Core;
using Generator.Cli.Metamodel;
using Generator.Cli.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Cli
{
	class Program
	{
		static void Main(string[] args)
		{
			var model = JsonSerializer.Deserialize<Model>(File.ReadAllText(args[0]));

			if (model == null)
			{
				Console.Error.WriteLine("Invalid Model");
				return;
			}

			var container = new ServiceCollection();

			// Add model elements to container
			container.AddSingleton<Model>(_ => model);
			container.AddSingleton<IEnumerable<Entity>>(_ => model.Entities);
			container.AddSingleton<IEnumerable<EntityAttribute>>(_ =>
				model.Entities.SelectMany(x => x.Attributes).ToList());
			container.AddSingleton<IEnumerable<Page>>(_ => model.Pages);

			var serviceProvider = container.BuildServiceProvider();

			// Now generate the code.
			var output = GetTemplateTypes().Select(x => InvokeTemplate(serviceProvider, x));

			var results = output.SelectMany(x => x);
			foreach (var result in results)
			{
				Console.WriteLine(result);
			}
		}

		private static IEnumerable<GenerationResult> Generate<T>(ITemplate<T> template)
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

		private static Type[] GetTemplateTypes()
		{
			return (
					from x in typeof(Program).Assembly.GetTypes()
					from z in x.GetInterfaces()
					let y = x.BaseType
					where y != null && y.IsGenericType
					where !x.IsAbstract && !x.IsInterface
					where typeof(ITemplate<>).IsAssignableFrom(y.GetGenericTypeDefinition()) || z.IsGenericType
						&& typeof(ITemplate<>).IsAssignableFrom(z.GetGenericTypeDefinition())
					select x)
				.ToArray();
		}

		private static MethodInfo _generateMethod = typeof(Program).GetMethod("Generate", BindingFlags.Static | BindingFlags.NonPublic)!;

		private static IEnumerable<GenerationResult> InvokeTemplate(IServiceProvider serviceProvider, Type type)
		{
			var template = ActivatorUtilities.CreateInstance(serviceProvider, type);
			var methodInfo = _generateMethod.MakeGenericMethod(type.GetInterface("ITemplate`1").GetGenericArguments().First());
			return (IEnumerable<GenerationResult>) methodInfo.Invoke(null, new[] {template});
		}
	}
}