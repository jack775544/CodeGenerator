using System.Collections.Generic;

namespace Generator.Core.Validation
{
	public abstract class AbstractValidationRule<T> : IValidationRule<T>
	{
		public T Model { get; set; }

		public abstract ValidationResult Validate();
		public abstract IEnumerable<T> MapObjects();
	}
}