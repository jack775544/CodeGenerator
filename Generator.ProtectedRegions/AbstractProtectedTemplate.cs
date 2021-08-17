using System;
using System.Collections.Generic;
using Generator.Core.Templates;

namespace Generator.ProtectedRegions
{
	public abstract class AbstractProtectedTemplate<T> : AbstractTemplate<T>, IProtectedTemplate
	{
		public List<ProtectedRegion> ProtectedRegions { get; set; } = new();

		private ProtectedRegion _currentRegion;

		public void StartProtected(string name, string startComment, string endComment, bool enabled = false)
		{
			if (_currentRegion != null)
			{
				AddError("Can't start a new protected region when one is already started\n" +
					$"File: {OutputPath}\n" +
					$"Existing Region Name: {_currentRegion.Name}\n" +
					$"Current Region Name: {name}");
				return;
			}

			_currentRegion = new ProtectedRegion(startComment, endComment, name, enabled);
			ProtectedRegions.Add(_currentRegion);
			WriteLine(_currentRegion.StartAsString());
		}

		public void StartProtected(string name, CommentType type, bool enabled = false)
		{
			var (start, end) = GetCommentMarkers(type);
			StartProtected(name, start, end, enabled);
		}

		public void EndProtected()
		{
			WriteLine(_currentRegion.EndAsString());
			_currentRegion = null;
		}

		public IEnumerable<(ProtectedRegion, string)> GetActiveRegions(string contents)
		{
			foreach (var region in ProtectedRegions)
			{
				var startMatch = region.MakeStartRegex().Match(contents);
				if (!startMatch.Success)
				{
					continue;
				}

				var endMatch = region.MakeEndRegex().Match(contents);
				if (!endMatch.Success)
				{
					AddError("Unable to find end of started protected region\n" +
						$"File: {OutputPath}\n" +
						$"Region Name: {region.Name}");
					continue;
				}

				var startEndIndex = startMatch.Index + startMatch.Length;
				var endStartIndex = endMatch.Index;
				var returnRegion = region.Clone();
				returnRegion.Enabled = true;
				yield return (returnRegion, contents.Substring(startEndIndex, endStartIndex - startEndIndex));
			}
		}

		private static (string, string) GetCommentMarkers(CommentType commentType)
		{
			return commentType switch
			{
				CommentType.DoubleSlash => ("//", ""),
				CommentType.SlashStar => ("/*", "*/"),
				CommentType.Xml => ("<!--", "-->"),
				_ => throw new ArgumentOutOfRangeException(nameof(commentType), commentType, null)
			};
		}
	}
}