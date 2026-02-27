using Warehouse.Features.OrderItems;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders
{
    public record OrderResponse(
        int Id,
        string OrderNumber,
        decimal? TotalAmount,
        OrderStatus Status,
        DateTime? CreatedAt,
        List<OrderItemResponse> Items
    );
}
