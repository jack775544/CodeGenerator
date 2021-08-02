using System.Text;

namespace Generator.Cli.Core
{
	public partial class BaseTemplate
	{
		public void ResetGenerationEnvironment()
		{
			GenerationEnvironment = new StringBuilder();
		}
	}
}