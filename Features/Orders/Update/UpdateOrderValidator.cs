using FluentValidation;

namespace Warehouse.Features.Orders.Update
{

    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid Order ID.");

            
        }
    }

}
