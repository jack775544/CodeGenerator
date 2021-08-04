using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Core.Validation
{
	public abstract class AbstractValidationRule<T> : IValidationRule<T> where T : IMetamodelNode
	{
		public T Model { get; set; }

		public abstract ValidationResult Validate();
		public abstract IEnumerable<T> MapObjects();
	}
}