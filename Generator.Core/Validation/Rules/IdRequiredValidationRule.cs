using System.Collections.Generic;
using Generator.Core.Metamodel;

namespace Generator.Core.Validation.Rules
{
	public class IdRequiredValidationRule : AbstractValidationRule<IMetamodelNode>
	{
		private readonly IEnumerable<IMetamodelNode> _metamodelNodes;

		public IdRequiredValidationRule(IEnumerable<IMetamodelNode> metamodelNodes)
		{
			_metamodelNodes = metamodelNodes;
		}
		
		public override ValidationResult Validate()
		{
			if (Model.Id == default)
			{
				return new FailedValidationResult($"{Model} is missing an ID");
			}

			return new SuccessfulValidationResult();
		}

		public override IEnumerable<IMetamodelNode> MapObjects()
		{
			return _metamodelNodes;
		}
	}
}