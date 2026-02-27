using FluentValidation;

namespace Warehouse.Features.OrderItems.Create
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemCommand>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be a valid positive number.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be a valid positive number.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");
        }
    }
}
