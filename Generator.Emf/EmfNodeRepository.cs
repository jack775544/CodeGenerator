using Generator.Emf.Utility;
using org.eclipse.emf.ecore;
using org.eclipse.emf.ecore.resource;

namespace Generator.Emf;

internal class EmfNodeRepository
{
	public ResourceSet ResourceSet { get; set; } = null!;

	public IEnumerable<EObject> GetNodes(Type type)
	{
		return ResourceSet.getResources()
			.GetEnumerable<Resource>()
			.SelectMany(x => x.getAllContents().GetEnumerable<EObject>())
			.Where(e => e.GetType().IsAssignableTo(type));
	}
}