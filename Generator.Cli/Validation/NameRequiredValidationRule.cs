using System;
using System.Collections.Generic;
using Generator.Cli.Metamodel;
using Generator.Core.Validation;

namespace Generator.Cli.Validation
{
	public class NameRequiredValidationRule : AbstractValidationRule<INamedNode>
	{
		private readonly IEnumerable<INamedNode> _entities;

		public NameRequiredValidationRule(IEnumerable<INamedNode> entities)
		{
			_entities = entities;
		}
		
		public override ValidationResult Validate()
		{
			if (string.IsNullOrWhiteSpace(Model.Name))
			{
				return new FailedValidationResult($"An entity with an id of {Model.Id} has a missing name");
			}

			return new SuccessfulValidationResult();
		}

		public override IEnumerable<INamedNode> MapObjects()
		{
			return _entities;
		}
	}
}