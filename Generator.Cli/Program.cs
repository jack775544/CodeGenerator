using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Generator.Cli.Metamodel;
using Generator.Core;
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
				.AddMetaModelType(_ => model.Entities)
				.AddMetaModelType(_ => model.Entities.SelectMany(x => x.Attributes).ToList())
				.AddMetaModelType(_ => model.Pages);

			// Now generate the code.
			var results = generator.GenerateAll();
			foreach (var result in results)
			{
				Console.WriteLine(result);
			}
		}
	}
}