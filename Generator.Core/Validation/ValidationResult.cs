namespace Generator.Core.Validation
{
	public class ValidationResult
	{
		public bool Succeeded { get; }
		
		public string Message { get; }

		internal ValidationResult(bool succeeded, string message)
		{
			Succeeded = succeeded;
			Message = message;
		}
	}

	public class SuccessfulValidationResult : ValidationResult
	{
		public SuccessfulValidationResult() : base(true, null)
		{
		}
	}

	public class FailedValidationResult : ValidationResult
	{
		public FailedValidationResult(string message) : base(false, message)
		{
		}
	}
}