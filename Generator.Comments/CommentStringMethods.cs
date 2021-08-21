using System;

namespace Generator.Comments
{
	public static class CommentStringMethods
	{
		public static (string, string) GetSingleLineCommentMarkers(CommentType commentType)
		{
			return commentType switch
			{
				CommentType.DoubleSlash => ("// ", ""),
				CommentType.SlashStar => ("/* ", " */"),
				CommentType.Xml => ("<!-- ", " -->"),
				_ => throw new ArgumentOutOfRangeException(nameof(commentType), commentType, null)
			};
		}
	}
}