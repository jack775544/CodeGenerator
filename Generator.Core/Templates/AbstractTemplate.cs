using System.Collections.Generic;

namespace Generator.Core.Templates
{
	public abstract class AbstractTemplate<T> : BaseTemplate, ITemplate<T>
	{
		public abstract string OutputPath { get; }

		public T Model { get; set; }

		public abstract IEnumerable<T> MapObjects();

		public bool Guard()
		{
			return true;
		}
	}
}