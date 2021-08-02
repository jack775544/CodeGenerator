using System.Collections.Generic;

namespace Generator.Cli.Core
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