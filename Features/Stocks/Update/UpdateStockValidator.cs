using FluentValidation;

namespace Warehouse.Features.Stocks.Update
{
    public class UpdateStockValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid positive number.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater.");
        }
    }
}
