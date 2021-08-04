using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Core.Validation
{
	public interface IValidationRule<T> where T : IMetamodelNode
	{
		T Model { get; set; }
		ValidationResult Validate();
		IEnumerable<T> MapObjects();
	}
}