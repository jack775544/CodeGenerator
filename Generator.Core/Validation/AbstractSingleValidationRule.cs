using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Core.Validation
{
	public abstract class AbstractSingleValidationRule : AbstractValidationRule<BaseModel>
	{
		public override IEnumerable<BaseModel> MapObjects()
		{
			return new BaseModel[] { null };
		}
	}
}