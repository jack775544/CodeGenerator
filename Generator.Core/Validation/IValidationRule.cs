using System.Collections.Generic;

namespace Generator.Core.Validation
{
	public interface IValidationRule<T>
	{
		T Model { get; set; }
		ValidationResult Validate();
		IEnumerable<T> MapObjects();
	}
}