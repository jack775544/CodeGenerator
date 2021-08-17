using System.Collections.Generic;
using Generator.Core.Templates;

namespace Generator.ProtectedRegions
{
	public interface IProtectedTemplate : ITextTemplate
	{
		List<ProtectedRegion> ProtectedRegions { get; set; }
		void StartProtected(string name, string startComment, string endComment, bool enabled = false);
		void StartProtected(string name, CommentType type, bool enabled = false);
		void EndProtected();
		IEnumerable<(ProtectedRegion, string)> GetActiveRegions(string contents);
	}
}