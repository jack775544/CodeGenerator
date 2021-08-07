using System;
using System.ComponentModel.DataAnnotations;

namespace Generator.Core.Metamodel
{
	public interface IMetamodelNode
	{
		[Required]
		Guid Id { get; set; }
	}
}