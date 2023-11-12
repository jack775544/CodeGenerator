using System;
using System.Collections.Generic;
using Generator.Core.Hooks;
using Microsoft.Extensions.DependencyInjection;

namespace Generator.Core;

public abstract class CodeGenerator
{
	private readonly IServiceProvider _serviceProvider;
	private readonly List<Type> _templateTypes;
	private readonly List<Type> _validationTypes;
	private readonly List<IGenerateHook> _hooks;

	public CodeGenerator(
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

	protected CodeGeneratorScope CreateGeneratorScopeInternal()
	{
		return new CodeGeneratorScope(
			_serviceProvider.CreateScope(),
			_templateTypes,
			_validationTypes,
			_hooks);
	}
}