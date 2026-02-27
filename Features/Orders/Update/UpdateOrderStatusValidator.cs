using FluentValidation;

namespace Warehouse.Features.Orders.Update
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid positive number.");

            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("Invalid status. Must be Pending, Processing, Completed, or Cancelled.");
        }
    }
}
