using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Exceptions
{
    public class ValidateException : Exception
    {
        public IEnumerable<ValidatorError> Errors { get; } = [];

        public ValidateException(string message) : base(message) { }

        public ValidateException(IEnumerable<ValidatorError> errors) : base("Validation Error")
        {
            Errors = errors;
        }
    }
}