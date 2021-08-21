using System.Collections.Generic;

namespace Generator.Clean
{
	public class CleanConfiguration
	{
		public int MaxMarkerLine = 5;

		public string Message = "@Generated This file has been automatically generated";

		public List<string> Markers = new()
		{
			"@Generated",
		};
	}
}