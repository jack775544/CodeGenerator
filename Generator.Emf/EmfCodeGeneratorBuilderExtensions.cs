using System.Collections;
using Generator.Core;
using Generator.Core.Utility;
using Generator.Emf.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using org.eclipse.emf.ecore;
using System.Reflection;

namespace Generator.Emf;

public static class EmfCodeGeneratorBuilderExtensions
{
	public static CodeGeneratorBuilder RegisterEPackage(this CodeGeneratorBuilder self, EPackage ePackage)
	{
		// Initialise the eClass
		ePackage.eClass();

		self.TryAddScoped<EmfNodeRepository>();

		var types = ePackage
			.getEClassifiers()
			.GetEnumerableOfType<EClass>()
			.Select(x => x.getInstanceClassName())
			.Select(x => ePackage.GetType().Assembly.GetType(x)!)
			.ToList();

		foreach (var type in types)
		{
			AddResource(self, type);
			foreach (var baseType in TypeHelpers.GetBaseTypes(type))
			{
				AddResource(self, baseType);
			}
		}

		return self;
	}

	public static EmfCodeGenerator BuildEmfGenerator(this CodeGeneratorBuilder self)
	{
		var sp = self.BuildServiceProvider();

		return new EmfCodeGenerator(
			sp,
			sp.GetRequiredService<TemplateTypeCollection>(),
			sp.GetRequiredService<ValidationTypeCollection>(),
			sp.GetRequiredService<GenerateHookCollection>());
	}

	private static void AddResource(IServiceCollection builder, Type type)
	{
		builder.TryAddScoped(
			typeof(IEnumerable<>).MakeGenericType(type),
			sp => TypeHelpers.CastRuntime(sp.GetRequiredService<EmfNodeRepository>().GetNodes(type), type));
	}
}