using System.Collections.Generic;
using System.Linq;

namespace Generator.Core
{
	public class GenerationResult
	{
		public string FileName { get; set; }
		public string Contents { get; set; }

		public List<string> Errors { get; set; }

		public GenerationResult(string fileName, string contents, IEnumerable<string> errors)
		{
			FileName = fileName;
			Contents = contents;
			Errors = errors.ToList();
		}

		public override string ToString()
		{
			return $"{FileName}: {Contents}";
		}
	}
}