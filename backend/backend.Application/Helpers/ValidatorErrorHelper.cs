using FluentValidation.Results;

namespace backend.Application.Helpers
{
    public class ValidatorErrorHelper
    {
		public required string PropertyName { get; set; }
		public required string ErrorMessage { get; set; }

		public static List<ValidatorErrorHelper> GetErrors(ValidationResult validationResult)
		{
			return validationResult.Errors.Select(x => new ValidatorErrorHelper
			{
				PropertyName = x.PropertyName,
				ErrorMessage = x.ErrorMessage
			}).ToList();
		}
    }
}