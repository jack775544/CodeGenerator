namespace Generator.Cli
{
	public class GenerationResult
	{
		public string FileName { get; set; }
		public string Contents { get; set; }

		public GenerationResult(string fileName, string contents)
		{
			FileName = fileName;
			Contents = contents;
		}

		public override string ToString()
		{
			return $"{FileName}: {Contents}";
		}
	}
}