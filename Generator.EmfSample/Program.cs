using au.com.JackBot2MetaModel;
using Generator.Core;
using Generator.Core.Validation;
using Generator.Emf;
using org.eclipse.emf.common.util;
using org.eclipse.emf.ecore.resource;
using org.eclipse.emf.ecore.resource.impl;
using org.eclipse.emf.ecore.xmi.impl;

var generator = new CodeGeneratorBuilder(typeof(Program).Assembly)
	.AutoWireTemplateTypes()
	.RegisterEPackage(JackBot2MetaModelPackage.eINSTANCE)
	.BuildEmfGenerator();

var reg = Resource.Factory.Registry.INSTANCE;
var m = reg.getExtensionToFactoryMap();
m.put("jackbotentity", new XMIResourceFactoryImpl());

var resSet = new ResourceSetImpl();
resSet.getResource(URI.createURI("model.jackbotentity"), true);

using var scope = generator.CreateGeneratorScope(resSet);

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