using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace Generator.Core.Templates
{
	public partial class BaseTemplate
	{
		public void ResetGenerationEnvironment()
		{
			GenerationEnvironment = new StringBuilder();
		}
		
		public StringBuilder GetGenerationEnvironment()
		{
			return GenerationEnvironment;
		}

		public void AddError(string error)
		{
			Errors.Add(new CompilerError { ErrorText = error});
		}

		public IEnumerable<string> GetErrors()
		{
			foreach (CompilerError error in Errors)
			{
				yield return error.ErrorText;
			}
		}
	}
}