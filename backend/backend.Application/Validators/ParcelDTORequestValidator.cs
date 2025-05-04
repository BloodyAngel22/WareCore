using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using FluentValidation;

namespace backend.Application.Validators
{
    public class ParcelDTORequestValidator : AbstractValidator<ParcelDTORequest>
    {
        public ParcelDTORequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required").Must(x => x > 0).WithMessage("CategoryId must be greater than 0");

            RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("Warehouse is required").Must(guid => guid != Guid.Empty).WithMessage("Warehouse cannot be Guid.Empty");

            RuleFor(x => x.Weight).NotEmpty().WithMessage("Weight is required").Must(x => x > 0 && x <= 100).WithMessage("Weight must be greater than 0 and less than or equal to 100");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100).WithMessage("Name must be less than 50 characters");
        }
    }
}