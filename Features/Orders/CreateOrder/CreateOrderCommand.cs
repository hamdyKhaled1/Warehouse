using MediatR;

namespace Warehouse.Features.Orders.CreateOrder
{
    public record CreateOrderCommand(
    List<OrderItemDto> Items
) : IRequest<int>;

    public record OrderItemDto(
        int ProductId,
        int Quantity
    );
}
