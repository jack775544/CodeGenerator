using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Clean;
using Generator.CleanSample.Metamodel;
using Generator.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.CleanSample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var generator = new CodeGeneratorBuilder<Model>(typeof(Program).Assembly)
				.AutoWireTemplateTypes()
				.AddFileCleanup()
				.AddMetamodelType<Page>()
				.Build();

			var model = new Model
			{
				Id = Guid.Parse("cff4ec10-7bb6-4850-9fe0-331c1f2734ff"),
				Pages = new List<Page>
				{
					new Page {Id = Guid.Parse("26763428-117a-42f5-8a40-a25f28d13d89"), StudentName = "Jack"},
					new Page {Id = Guid.Parse("eebe374b-5d1a-470e-81f0-23152d1c5651"), StudentName = "Tom"},
				},
			};

			using var scope = generator.CreateGeneratorScope(model);
			var results = scope.GenerateAll();

			Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Output"));
			var newFiles = new HashSet<string>();
			foreach (var result in results)
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), "Output", result.FileName);
				newFiles.Add(path);
				File.WriteAllText(path, result.Contents);
			}

			var oldFiles = scope.GetRequiredService<OldFileFinder>()
				.FindOldFiles(
					Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Output"), "*.*", SearchOption.AllDirectories).Select(x => new FileRecord(x)),
					newFiles)
				.ToList();

			foreach (var file in oldFiles)
			{
				File.Delete(file);
			}
		}
	}

	public class FileRecord : IFileInfo
	{
		public string Name { get; }

		public FileRecord(string name)
		{
			Name = name;
		}

		public Stream GetContents()
		{
			return File.OpenRead(Name);
		}
	}
}