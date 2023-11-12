using System.Collections.Generic;

namespace Generator.Core.Validation
{
	public abstract class AbstractSingleValidationRule : AbstractValidationRule<object>
	{
		public override IEnumerable<object> MapObjects()
		{
			return new object[] { null };
		}
	}
}