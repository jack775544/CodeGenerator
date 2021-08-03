using System.Text;

namespace Generator.Core.Templates
{
	public partial class BaseTemplate
	{
		public void ResetGenerationEnvironment()
		{
			GenerationEnvironment = new StringBuilder();
		}
	}
}