using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.OrderItems.Get
{
    public record GetOrderItemsByOrderIdQuery(int OrderId)
         : IRequest<Result<List<OrderItemResponse>>>;
}
