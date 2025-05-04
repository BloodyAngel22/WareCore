using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using FluentValidation;

namespace backend.Application.Validators
{
    public class CategoryDTORequestValidator : AbstractValidator<CategoryDTORequest>
    {
        public CategoryDTORequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(50).WithMessage("Name must be less than 50 characters");
        }
    }
}