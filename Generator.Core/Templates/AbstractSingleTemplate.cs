using System.Collections.Generic;

namespace Generator.Core.Templates
{
	public abstract class AbstractSingleTemplate : AbstractTemplate<object?>
	{
		public override IEnumerable<object?> MapObjects()
		{
			return new object?[] { null };
		}
	}
}