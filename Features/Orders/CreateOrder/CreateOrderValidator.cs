using FluentValidation;

namespace Warehouse.Features.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            

            RuleFor(x => x.Items)
                 .NotEmpty().WithMessage("I Can not Create Order Without Items.")
                 .Must(items => items != null && items.Count > 0).WithMessage("Add one Product At Least.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).GreaterThan(0);
                item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("The Quantity Must Be Bigger than 0.");
                item.RuleFor(i => i.UnitPrice).GreaterThan(0).WithMessage("The Price Must Be Bigger than 0.");
            });
        }
    }
}
