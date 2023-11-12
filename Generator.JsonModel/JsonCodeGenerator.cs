using System;
using System.Collections.Generic;
using Generator.Core;
using Generator.Core.Hooks;

namespace Generator.JsonModel;

public class JsonCodeGenerator : CodeGenerator
{
	public JsonCodeGenerator(
		IServiceProvider serviceProvider,
		List<Type> templateTypes,
		List<Type> validationTypes,
		List<IGenerateHook> hooks)
		: base(serviceProvider, templateTypes, validationTypes, hooks)
	{
	}

	public CodeGeneratorScope CreateGeneratorScope()
	{
		var scope = CreateGeneratorScopeInternal();
		return scope;
	}
}