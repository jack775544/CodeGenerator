using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Cli.Metamodel
{
	public class Model : BaseModel
	{
		public List<Entity> Entities { get; set; }
		public List<Page> Pages { get; set; }
	}
}