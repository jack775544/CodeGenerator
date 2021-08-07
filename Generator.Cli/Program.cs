using System;
using System.Collections.Generic;
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
			var model = JsonModelDeserializer.Deserialize<Model>(File.ReadAllText(args[0]));
			if (model == null)
			{
				Console.Error.WriteLine("Invalid Model");
				return;
			}

			// Create the generator
			var generator = new CodeGeneratorBuilder<Model>(typeof(Program).Assembly)
				.AutoWireTemplateTypes()
				.AutoWireValidationTypes()
				.AddMetamodelType<Entity>()
				.AddMetamodelType<Page>()
				.AddMetamodelType<AttributeString>()
				.AddMetamodelType<AttributeBoolean>()
				.AddMetamodelType<ReferenceOneToMany>(builder =>
				{
					builder
						.HasOne(e => e.Source)
						.WithMany(e => e.OutgoingOneToManyReferences);
					builder
						.HasOne(e => e.Target)
						.WithMany(e => e.IncomingOneToManyReferences);
				})
				.AddMetamodelType<ReferenceOneToOne>(builder =>
				{
					builder
						.HasOne(e => e.Source)
						.WithMany(e => e.OutgoingOneToOneReferences);
					builder
						.HasOne(e => e.Target)
						.WithMany(e => e.IncomingOneToOneReferences);
				})
				.AddMetamodelType<ReferenceManyToMany>(builder =>
				{
					builder
						.HasOne(e => e.Source)
						.WithMany(e => e.OutgoingManyToManyReferences);
					builder
						.HasOne(e => e.Target)
						.WithMany(e => e.IncomingManyToManyReferences);
				})
				.Build();

			using var scope = generator.CreateGeneratorScope(model);

			// Validate the model
			var failedValidationResults = scope.ValidateAll()
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
			var results = scope.GenerateAll();
			foreach (var result in results)
			{
				Console.WriteLine(result);
			}
		}
	}
}