// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Generator.Core.Metamodel;
//
// namespace Generator.Core.Validation.Rules
// {
// 	public class UniqueIdValidator : AbstractSingleValidationRule
// 	{
// 		private readonly List<IMetamodelNode> _nodes;
//
// 		public UniqueIdValidator(IEnumerable<IMetamodelNode> nodes)
// 		{
// 			_nodes = nodes.ToList();
// 		}
//
// 		public override ValidationResult Validate()
// 		{
// 			var set = new HashSet<Guid>();
// 			var duplicates = new HashSet<string>();
//
// 			foreach (var node in _nodes)
// 			{
// 				if (set.Add(node.Id) == false)
// 				{
// 					duplicates.Add(node.Id.ToString());
// 				}
// 			}
//
// 			if (!duplicates.Any())
// 			{
// 				return new SuccessfulValidationResult();
// 			}
//
// 			var idString = string.Join(", ", duplicates);
// 			return new FailedValidationResult($"The ids {idString} are duplicated. All model ids must be unique.");
//
// 		}
// 	}
// }