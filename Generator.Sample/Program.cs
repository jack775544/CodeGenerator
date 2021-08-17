using System;
using System.IO;
using System.Linq;
using Generator.Core;
using Generator.Core.Validation;
using Generator.JsonModel;
using Generator.Sample.Metamodel;

namespace Generator.Sample
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
				.AddMetamodelType<EntityAttribute>()
				.AddMetamodelType<AttributeString>()
				.AddMetamodelType<AttributeBoolean>()
				.AddMetamodelType<AttributeDateTime>()
				.AddMetamodelType<Reference>(builder =>
				{
					builder
						.HasOne(e => e.Source)
						.WithMany(e => e.OutgoingReferences);
					builder
						.HasOne(e => e.Target)
						.WithMany(e => e.IncomingReferences);
				})
				.AddMetamodelType<ReferenceOneToMany>()
				.AddMetamodelType<ReferenceOneToOne>()
				.AddMetamodelType<ReferenceManyToMany>()
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