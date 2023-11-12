using Generator.Core;
using Generator.Core.Hooks;
using Microsoft.Extensions.DependencyInjection;
using org.eclipse.emf.ecore.resource;

namespace Generator.Emf;

public class EmfCodeGenerator : CodeGenerator
{
	internal EmfCodeGenerator(
		IServiceProvider serviceProvider,
		List<Type> templateTypes,
		List<Type> validationTypes,
		List<IGenerateHook> hooks) : base(serviceProvider, templateTypes, validationTypes, hooks)
	{
	}

	public CodeGeneratorScope CreateGeneratorScope(ResourceSet resourceSet)
	{
		var scope = CreateGeneratorScopeInternal();
		scope.ServiceProvider.GetRequiredService<EmfNodeRepository>().ResourceSet = resourceSet;
		return scope;
	}
}