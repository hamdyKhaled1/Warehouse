using FluentValidation;

namespace Warehouse.Features.Stocks.Create
{
    public class CreateStockValidator : AbstractValidator<CreateStockCommand>
    {
        public CreateStockValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be a valid positive number.");

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0).WithMessage("WarehouseId must be a valid positive number.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater.");
        }
    }
}
