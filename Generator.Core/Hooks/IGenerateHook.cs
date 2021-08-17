using Generator.Core.Templates;

namespace Generator.Core.Hooks
{
	public interface IGenerateHook
	{
		void AfterGenerate(ITextTemplate template);
	}
}