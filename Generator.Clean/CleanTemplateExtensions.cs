using Generator.Comments;
using Generator.Core.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Generator.Clean
{
	public static class CleanTemplateExtensions
	{
		private const string CleanMetadataKey = "CleanHeaderExists";

		public static string AddCleanHeader(this ITextTemplate template, string startMarker, string endMarker)
		{
			var configuration = template.ServiceProvider.GetRequiredService<IOptions<CleanConfiguration>>();
			template.AddCleanMetadata();
			return $"{startMarker}{configuration.Value.Message}{endMarker}";
		}

		public static string AddCleanHeader(this ITextTemplate template, CommentType type)
		{
			var (start, end) = CommentStringMethods.GetSingleLineCommentMarkers(type);
			return template.AddCleanHeader(start, end);
		}

		internal static void AddCleanMetadata(this ITextTemplate template)
		{
			template.Metadata[CleanMetadataKey] = true;
		}

		internal static bool CleanHeaderExists(this ITextTemplate template)
		{
			return template.Metadata.TryGetValue(CleanMetadataKey, out _);
		}
	}
}