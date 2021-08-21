using System.Collections.Generic;
using System.IO;
using Generator.Core;
using Microsoft.Extensions.Options;

namespace Generator.Clean
{
	public interface IFileInfo
	{
		string Name { get; }
		Stream GetContents();
	}
	
	public class OldFileFinder
	{
		private readonly IOptions<CleanConfiguration> _configuration;

		public OldFileFinder(IOptions<CleanConfiguration> configuration)
		{
			_configuration = configuration;
		}
		
		public IEnumerable<string> FindOldFiles(IEnumerable<IFileInfo> oldFiles, ISet<string> newFiles)
		{
			foreach (var oldFile in oldFiles)
			{
				if (newFiles.Contains(oldFile.Name)) continue;

				var found = false;
				foreach (var marker in _configuration.Value.Markers)
				{
					if (found) continue;
					using var contents = oldFile.GetContents();
					using var reader = new StreamReader(contents);
					for (var i = 0; i < _configuration.Value.MaxMarkerLine; i++)
					{
						if (found) continue;
						var line = reader.ReadLine();
						if (line == null)
						{
							break;
						}

						if (line.Contains(marker))
						{
							yield return oldFile.Name;
							found = true;
						}
					}
				}
			}
		}
	}
}