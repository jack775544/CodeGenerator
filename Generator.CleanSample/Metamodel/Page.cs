using System;
using Generator.Core.Metamodel;

namespace Generator.CleanSample.Metamodel
{
	public class Page : IMetamodelNode
	{
		public Guid Id { get; set; }
		public string StudentName { get; set; }
	}
}