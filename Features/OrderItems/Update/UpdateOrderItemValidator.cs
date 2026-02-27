//using FluentValidation;

//namespace Warehouse.Features.OrderItems.Update
//{
//    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemCommand>
//    {
//        public UpdateOrderItemValidator()
//        {
//            RuleFor(x => x.Id)
//                .GreaterThan(0).WithMessage("Id must be a valid positive number.");

//            RuleFor(x => x.Quantity)
//                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

//            RuleFor(x => x.UnitPrice)
//                .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");
//        }
//    }
//}
