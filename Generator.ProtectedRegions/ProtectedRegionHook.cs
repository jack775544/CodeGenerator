using System;
using Generator.Core.Hooks;
using Generator.Core.Templates;

namespace Generator.ProtectedRegions
{
	public class ProtectedRegionHook : IGenerateHook
	{
		private readonly Func<ITextTemplate, string> _getExistingContents;

		public ProtectedRegionHook(Func<ITextTemplate, string> getExistingContents)
		{
			_getExistingContents = getExistingContents;
		}
		
		public void Intercept(ITextTemplate template)
		{
			if (template is not IProtectedTemplate protectedTemplate)
			{
				return;
			}

			var existingContents = _getExistingContents(template);
			if (existingContents == null)
			{
				return;
			}

			var templateOutput = template.GetGenerationEnvironment();
			var regions = protectedTemplate.GetActiveRegions(existingContents);
			foreach (var (region, contents) in regions)
			{
				var templateOutputString = templateOutput.ToString();
				var (idx, length) = ProtectedRegionHelpers.FindProtectedRegionContentIndex(templateOutputString, region);
				templateOutput.Remove(idx, length);
				templateOutput.Insert(idx, region.StartAsString() + contents + region.EndAsString());
			}
		}
	}
}