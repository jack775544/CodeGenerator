using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Generator.Cli.Metamodel;
using Generator.Core;
using Generator.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Cli
{
	public class Program
	{
		static void Main(string[] args)
		{
			var model = JsonSerializer.Deserialize<Model>(File.ReadAllText(args[0]));

			if (model == null)
			{
				Console.Error.WriteLine("Invalid Model");
				return;
			}

			var generator = new CodeGenerator<Model>(model, typeof(Program).Assembly)
				.AutoWireTemplateTypes()
				.AutoWireValidationTypes()
				.AddMetaModelType(_ => model.Entities)
				.AddMetaModelType(_ => model.Entities.SelectMany(x => x.Attributes).ToList())
				.AddMetaModelType(_ => model.Pages);
			generator._serviceCollection.BuildServiceProvider().GetRequiredService<IEnumerable<INamedNode>>();

			var failedValidationResults = generator.ValidateAll()
				.Where(x => x is FailedValidationResult)
				.ToList();

			if (failedValidationResults.Any())
			{
				foreach (var result in failedValidationResults)
				{
					Console.Error.WriteLine(result.Message);
				}

				return;
			}

			// Now generate the code.
			var results = generator.GenerateAll();
			foreach (var result in results)
			{
				Console.WriteLine(result);
			}
		}
	}
}