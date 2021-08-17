using System;
using System.Collections.Generic;
using Generator.Core.Hooks;
using Generator.Core.Metamodel;
using Generator.Core.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core
{
	public class CodeGenerator<T> where T : BaseModel
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly List<Type> _templateTypes;
		private readonly List<Type> _validationTypes;
		private readonly List<IGenerateHook> _hooks;

		internal CodeGenerator(
			IServiceProvider serviceProvider,
			List<Type> templateTypes,
			List<Type> validationTypes,
			List<IGenerateHook> hooks)
		{
			_serviceProvider = serviceProvider;
			_templateTypes = templateTypes;
			_validationTypes = validationTypes;
			_hooks = hooks;
		}

		public CodeGeneratorScope<T> CreateGeneratorScope(T model)
		{
			var scope = new CodeGeneratorScope<T>(
				_serviceProvider.CreateScope(),
				_templateTypes,
				_validationTypes,
				_hooks);
			var databaseIdentifier = scope.ServiceProvider.GetRequiredService<DatabaseIdentifier>();
			databaseIdentifier.DatabaseId = Guid.NewGuid().ToString();
			var repository = scope.ServiceProvider.GetRequiredService<NodeRepository>();
			repository.Add(model);
			repository.SaveChanges();
			return scope;
		}
	}
}