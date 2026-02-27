using MediatR;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Update
{
    public record UpdateOrderStatusCommand(
        int Id,
        OrderStatus NewStatus
    ) : IRequest<Result<OrderResponse>>;
}
