using System.Collections.Generic;
using Generator.Core.Templates;

namespace Generator.ProtectedRegions
{
	public interface IProtectedTemplate : ITextTemplate
	{
		List<ProtectedRegion> ProtectedRegions { get; set; }
		string StartProtected(string name, string startComment, string endComment, bool enabled = false);
		string StartProtected(string name, CommentType type, bool enabled = false);
		string EndProtected();
		IEnumerable<(ProtectedRegion, string)> GetActiveRegions(string contents);
	}
}