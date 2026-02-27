using FluentValidation;

namespace Warehouse.Features.Warehouses.Update
{
    public class UpdateWarehouseValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid positive number.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
        }
    }
}
