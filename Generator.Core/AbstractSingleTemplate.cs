using System.Collections.Generic;

namespace Generator.Core
{
	public abstract class AbstractSingleTemplate : AbstractTemplate<object?>
	{
		public override IEnumerable<object?> MapObjects()
		{
			return new object?[] { null };
		}
	}
}