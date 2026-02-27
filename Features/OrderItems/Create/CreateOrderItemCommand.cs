using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.OrderItems.Create
{
    public record CreateOrderItemCommand(
        int OrderId,
        int ProductId,
        int Quantity,
        decimal UnitPrice
    ) : IRequest<Result<OrderItemResponse>>;
}
