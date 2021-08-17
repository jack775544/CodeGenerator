using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Sample.Metamodel
{
	public class Model : BaseModel
	{
		public virtual List<Entity> Entities { get; set; }
		public virtual List<Page> Pages { get; set; }
		public virtual List<Reference> References { get; set; }
	}
}