using System;
using System.Collections.Generic;
using System.IO;
using Generator.Core;
using Generator.Core.Templates;
using Generator.ProtectedRegions;
using Generator.ProtectedSample.Metamodel;

namespace Generator.ProtectedSample
{
	class Program
	{
		public static void Main(string[] args)
		{
			var generator = new CodeGeneratorBuilder<Model>(typeof(Program).Assembly)
				.AutoWireTemplateTypes()
				.AddMetamodelType<Page>()
				.AddGenerationHook(new ProtectedRegionHook(GetExistingContents))
				.Build();

			var model = new Model
			{
				Id = Guid.Parse("cff4ec10-7bb6-4850-9fe0-331c1f2734ff"),
				Pages = new List<Page>
				{
					new Page {Id = Guid.Parse("26763428-117a-42f5-8a40-a25f28d13d89"), StudentName = "Jack"},
					new Page {Id = Guid.Parse("eebe374b-5d1a-470e-81f0-23152d1c5651"), StudentName = "Tom"},
				}
			};

			using var scope = generator.CreateGeneratorScope(model);
			var results = scope.GenerateAll();

			Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Output"));
			foreach (var result in results)
			{
				File.WriteAllText(
					Path.Combine(Directory.GetCurrentDirectory(), "Output", result.FileName),
					result.Contents);
			}
		}

		private static string GetExistingContents(ITextTemplate template)
		{
			try
			{
				return File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Output", template.OutputPath));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}
	}
}