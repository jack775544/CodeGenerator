using System;
using System.IO;
using System.Linq;
using Generator.Cli.Metamodel;
using Generator.Core;
using Generator.Core.Validation;
using Generator.JsonModel;

namespace Generator.Cli
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var deserializerResult = JsonModelDeserializer.Deserialize<Model>(File.ReadAllText(args[0]));
			if (deserializerResult.Model == null)
			{
				Console.Error.WriteLine("Invalid Model");
				return;
			}

			// Create the generator
			var generator = new CodeGenerator<Model>(deserializerResult.Model, deserializerResult.Nodes, typeof(Program).Assembly)
				.AutoWireTemplateTypes()
				.AutoWireValidationTypes()
				.AddMetaModelType(_ => deserializerResult.Model.Entities)
				.AddMetaModelType(_ => deserializerResult.Model.Entities.SelectMany(x => x.Attributes).ToList())
				.AddMetaModelType(_ => deserializerResult.Model.Pages);

			// Validate the model
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

			// Generate the code
			var results = generator.GenerateAll();
			foreach (var result in results)
			{
				Console.WriteLine(result);
			}
		}
	}
}