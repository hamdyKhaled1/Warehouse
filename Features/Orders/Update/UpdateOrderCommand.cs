using MediatR;
using Warehouse.Common;
using Warehouse.Features.OrderItems;
using Warehouse.Features.Orders.CreateOrder;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Update
{
    
    public record UpdateOrderItemDto(int ProductId, int Quantity, decimal UnitPrice);

    public record UpdateOrderCommand(
        int Id,
        List<UpdateOrderItemDto> Items 
    ) : IRequest<Result<OrderResponse>>;
}
