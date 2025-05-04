using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace backend.Core.Exceptions
{
    public class ValidatorError
    {
        public required string ErrorMessage { get; set; }

        public static List<ValidatorError> GetErrors(ValidationResult validationResult)
        {
            return validationResult.Errors.Select(x => new ValidatorError
            {
                ErrorMessage = x.ErrorMessage
            }).ToList();
        }

        public static string GetErrorsString(List<ValidatorError> errors)
        {
            return string.Join(", ", errors.Select(e => $"{e.ErrorMessage}"));
        }
    }
}