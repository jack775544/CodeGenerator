using System.Collections.Generic;
using System.Linq;
using Generator.Cli.Metamodel;
using Generator.Core.Validation;

namespace Generator.Cli.Validation
{
	public class NoDanglingReferenceRule : AbstractValidationRule<Reference>
	{
		private readonly IEnumerable<Reference> _references;

		public NoDanglingReferenceRule(IEnumerable<Reference> references)
		{
			_references = references;
		}

		public override ValidationResult Validate()
		{
			var errors = new List<string>();
			if (Model.SourceId == default || Model.Source == null)
			{
				errors.Add($"The reference {Model.Id} has a missing source.");
			}
			if (Model.TargetId == default || Model.Target == null)
			{
				errors.Add($"The reference {Model.Id} has a missing target.");
			}

			if (!errors.Any())
			{
				return new SuccessfulValidationResult();
			}

			return new FailedValidationResult(string.Join(" ", errors));
		}

		public override IEnumerable<Reference> MapObjects()
		{
			return _references;
		}
	}
}