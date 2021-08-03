using System.Collections.Generic;

namespace Generator.Core.Templates
{
	public interface ITemplate<T>
	{
		string OutputPath { get; }
		T Model { get; set; }
		IEnumerable<T> MapObjects();
		bool Guard();
		string TransformText();
		void ResetGenerationEnvironment();
	}
}