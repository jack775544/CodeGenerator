using System.Collections.Generic;
using System.Text;

namespace Generator.Core.Templates
{
	public interface ITextTemplate
	{
		string OutputPath { get; }
		bool Guard();
		string TransformText();
		void ResetGenerationEnvironment();
		StringBuilder GetGenerationEnvironment();
		void Write(string textToAppend);
		void WriteLine(string textToAppend);
		void AddError(string error);
		IEnumerable<string> GetErrors();
	}
}