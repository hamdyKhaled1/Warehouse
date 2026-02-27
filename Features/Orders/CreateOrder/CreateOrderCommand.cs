using MediatR;
using Warehouse.Common;
using Warehouse.Features.OrderItems;

namespace Warehouse.Features.Orders.CreateOrder
{
    //public record CreateOrderCommand(
    //        string OrderNumber,
    //        decimal? TotalAmount
    //    ) : IRequest<Result<OrderResponse>>;

    // كائن يمثل العنصر الواحد داخل الطلب
    public record OrderItemDto(int ProductId, int Quantity, decimal UnitPrice);

    public record CreateOrderCommand(
            List<OrderItemDto> Items // إجبار وجود عناصر
        ) : IRequest<Result<OrderResponse>>;
}
