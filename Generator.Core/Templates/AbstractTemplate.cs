using System.Collections.Generic;

namespace Generator.Core.Templates
{
	public abstract class AbstractTemplate<T> : BaseTemplate, ITemplate<T>
	{
		public abstract string OutputPath { get; }

		public T Model { get; set; }

		public IDictionary<string, object> Metadata { get; } = new Dictionary<string, object>();

		public abstract IEnumerable<T> MapObjects();

		public virtual bool Guard()
		{
			return true;
		}
	}
}