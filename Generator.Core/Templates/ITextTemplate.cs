using System;
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
		void AddError(string error);
		IEnumerable<string> GetErrors();
		IDictionary<string, object> Metadata { get; }
		IServiceProvider ServiceProvider { get; set; }
	}
}