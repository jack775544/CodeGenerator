using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			container.AddSingleton<IEnumerable<EntityAttribute>>(_ => model.Entities.SelectMany(x => x.Attributes).ToList());
			container.AddSingleton<IEnumerable<Page>>(_ => model.Pages);

			var serviceProvider = container.BuildServiceProvider();
			
			// Now generate the code.
			var output = new List<IEnumerable<GenerationResult>>
			{
				Generate(ActivatorUtilities.CreateInstance<EntityTemplate>(serviceProvider)),
				Generate(ActivatorUtilities.CreateInstance<PageTemplate>(serviceProvider)),
				Generate(ActivatorUtilities.CreateInstance<Helpers>(serviceProvider)),
			};

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
	}
}