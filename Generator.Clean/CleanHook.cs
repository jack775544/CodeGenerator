using Generator.Core.Hooks;
using Generator.Core.Templates;

namespace Generator.Clean
{
	public class CleanHook : IGenerateHook
	{
		public void AfterGenerate(ITextTemplate template)
		{
			if (!template.CleanHeaderExists())
			{
				template.AddError("Missing clean header");
			}
		}
	}
}