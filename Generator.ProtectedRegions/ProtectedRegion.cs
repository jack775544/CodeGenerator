using System.Text.RegularExpressions;

namespace Generator.ProtectedRegions
{
	public class ProtectedRegion
	{
		public string StartComment { get; set; }
		public string EndComment { get; set; }
		public string Name { get; set; }
		public bool Enabled { get; set; }

		public ProtectedRegion(string startComment, string endComment, string name, bool enabled)
		{
			StartComment = startComment;
			EndComment = endComment;
			Name = name;
			Enabled = enabled;
		}

		private string EnabledString => Enabled ? "on" : "off";

		public string StartAsString()
		{
			return $"{StartComment}{Name} {EnabledString} begin{EndComment}";
		}

		public string EndAsString()
		{
			return $"{StartComment}{Name} end{EndComment}";
		}

		internal Regex MakeStartRegex(bool enabled = true)
		{
			var start = Regex.Escape(StartComment);
			var end = Regex.Escape(EndComment);
			var name = Regex.Escape(Name);
			var enabledString = enabled ? "on" : "off";
			return new Regex($"{start}{name} {enabledString} begin{end}");
		}

		internal Regex MakeEndRegex()
		{
			var start = Regex.Escape(StartComment);
			var end = Regex.Escape(EndComment);
			var name = Regex.Escape(Name);
			return new Regex($"{start}{name} end{end}");
		}

		public ProtectedRegion Clone()
		{
			return new(StartComment, EndComment, Name, Enabled);
		}
	}
}