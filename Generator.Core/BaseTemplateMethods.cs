using System.Text;

namespace Generator.Core
{
	public partial class BaseTemplate
	{
		public void ResetGenerationEnvironment()
		{
			GenerationEnvironment = new StringBuilder();
		}
	}
}