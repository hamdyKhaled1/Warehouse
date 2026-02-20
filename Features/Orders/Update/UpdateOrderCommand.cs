using MediatR;
using Warehouse.Features.Orders.CreateOrder;

namespace Warehouse.Features.Orders.Update
{
    public record UpdateOrderCommand(
     int Id,
     string Status,
     List<OrderItemDto>? UpdatedItems = null
 ) : IRequest;
}
