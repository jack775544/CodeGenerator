using System.Collections.Generic;
using Generator.Comments;
using Generator.Core.Templates;

namespace Generator.ProtectedRegions;

public static class ProtectedTemplateExtensions
{
	#region PublicMethods

	public static string StartProtected(
		this ITextTemplate self,
		string name,
		string startComment,
		string endComment,
		bool enabled = false)
	{
		var currentRegion = self.GetCurrentProtectedRegion();
		if (currentRegion != null)
		{
			self.AddError("Can't start a new protected region when one is already started\n" +
						$"File: {self.OutputPath}\n" +
						$"Existing Region Name: {currentRegion.Name}\n" +
						$"Current Region Name: {name}");
			return "";
		}

		var region = new ProtectedRegion(startComment, endComment, name, enabled);
		self.SetCurrentProtectedRegion(region);
		self.AddProtectedRegion(region);
		return region.StartAsString();
	}

	public static string StartProtected(
		this ITextTemplate self,
		string name,
		CommentType type,
		bool enabled = false)
	{
		var (start, end) = CommentStringMethods.GetSingleLineCommentMarkers(type);
		return self.StartProtected(name, start, end, enabled);
	}

	public static string EndProtected(this ITextTemplate self)
	{
		var currentRegion = self.GetCurrentProtectedRegion();

		if (currentRegion == null)
		{
			self.AddError("Can't end a protected region since one has not already been started\n" +
						$"File: {self.OutputPath}\n");
			return "";
		}

		var end = currentRegion.EndAsString();
		self.SetCurrentProtectedRegion(null);
		return end;
	}

	public static IEnumerable<(ProtectedRegion, string)> GetActiveRegions(this ITextTemplate self, string contents)
	{
		foreach (var region in self.ProtectedRegions())
		{
			var startMatch = region.MakeStartRegex().Match(contents);
			if (!startMatch.Success)
			{
				continue;
			}

			var endMatch = region.MakeEndRegex().Match(contents);
			if (!endMatch.Success)
			{
				self.AddError("Unable to find end of started protected region\n" +
							$"File: {self.OutputPath}\n" +
							$"Region Name: {region.Name}");
				continue;
			}

			var startEndIndex = startMatch.Index + startMatch.Length;
			var endStartIndex = endMatch.Index;
			var returnRegion = region.Clone(true);
			yield return (returnRegion, contents.Substring(startEndIndex, endStartIndex - startEndIndex));
		}
	}

	#endregion

	#region InternalMethods

	internal const string ProtectedRegionMetadataKey = "AllProtectedRegions";
	internal const string CurrentProtectedRegionMetadataKey = "CurrentProtectedRegions";

	internal static List<ProtectedRegion> ProtectedRegions(this ITextTemplate self)
	{
		if (self.Metadata.TryGetValue(ProtectedRegionMetadataKey, out var regions))
		{
			return (List<ProtectedRegion>) regions;
		}

		var newRegions = new List<ProtectedRegion>();
		self.Metadata[ProtectedRegionMetadataKey] = newRegions;
		return newRegions;
	}

	internal static void AddProtectedRegion(this ITextTemplate self, ProtectedRegion region)
	{
		self.ProtectedRegions().Add(region);
	}

	internal static ProtectedRegion? GetCurrentProtectedRegion(this ITextTemplate self)
	{
		if (self.Metadata.TryGetValue(CurrentProtectedRegionMetadataKey, out var region))
		{
			return (ProtectedRegion) region;
		}

		return null;
	}

	internal static void SetCurrentProtectedRegion(this ITextTemplate self, ProtectedRegion? region)
	{
		self.Metadata[CurrentProtectedRegionMetadataKey] = region;
	}

	#endregion
}