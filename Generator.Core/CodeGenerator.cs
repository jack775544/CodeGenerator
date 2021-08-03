using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core
{
	public class CodeGenerator<TModel> where TModel : class
	{
		private readonly Assembly _generatingAssembly;
		private readonly IServiceCollection _serviceCollection;

		public CodeGenerator(TModel model, Assembly generatingAssembly)
		{
			_generatingAssembly = generatingAssembly;
			_serviceCollection = new ServiceCollection();
			_serviceCollection.AddSingleton(_ => model);
		}

		public CodeGenerator<TModel> AddMetaModelType<T>(
			Func<IServiceProvider, IEnumerable<T>> implementationFactory)
			where T : class
		{
			_serviceCollection.AddSingleton(implementationFactory);
			return this;
		}

		public IEnumerable<GenerationResult> GenerateAll()
		{
			var serviceProvider = _serviceCollection.BuildServiceProvider();
			return GenerateHelpers.GetTemplateTypes(_generatingAssembly)
				.Select(x => GenerateHelpers.InvokeTemplate(serviceProvider, x))
				.SelectMany(x => x);
		}
		
		public static IEnumerable<GenerationResult> Generate<T>(ITemplate<T> template)
		{
			return GenerateHelpers.Generate(template);
		}
	}
}