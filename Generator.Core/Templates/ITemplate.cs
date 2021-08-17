using System.Collections.Generic;

namespace Generator.Core.Templates
{
	public interface ITemplate<T> : ITextTemplate
	{
		T Model { get; set; }
		IEnumerable<T> MapObjects();
	}
}