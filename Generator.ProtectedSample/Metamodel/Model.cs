using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.ProtectedSample.Metamodel
{
	public class Model : BaseModel
	{
		public virtual List<Page> Pages { get; set; }
	}
}