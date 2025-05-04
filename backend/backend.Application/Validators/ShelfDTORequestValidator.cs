using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Application.DTOs.Request;
using FluentValidation;

namespace backend.Application.Validators
{
    public class ShelfDTORequestValidator : AbstractValidator<ShelfDTORequest>
    {
        public ShelfDTORequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required").Must(x => x > 0).WithMessage("CategoryId must be greater than 0");

            RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("Warehouse is required").Must(guid => guid != Guid.Empty).WithMessage("Warehouse cannot be Guid.Empty");

            RuleFor(x => x.ShelfWeight).NotEmpty().WithMessage("Shelf weight is required").Must(x => x > 0 && x <= 2048).WithMessage("Shelf weight must be greater than 0 and less than or equal to 2048");
        }
    }
}