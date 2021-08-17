using System;

namespace Generator.ProtectedRegions
{
	public class ProtectedRegionHelpers
	{
		public static (int, int) FindProtectedRegionContentIndex(string contents, ProtectedRegion region)
		{
			var startMatch = region.MakeStartRegex(false).Match(contents);
			if (!startMatch.Success)
			{
				throw new InvalidOperationException("Unable to find protected region");
			}s

			var endMatch = region.MakeEndRegex().Match(contents);
			if (!startMatch.Success)
			{
				throw new InvalidOperationException("Unable to find protected region end");
			}

			return (startMatch.Index, (endMatch.Index + endMatch.Length) - startMatch.Index);
		}
	}
}