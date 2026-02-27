namespace Warehouse.Features.OrderItems
{
    public record OrderItemResponse(
        int Id,
        int OrderId,
        int ProductId,
        int Quantity,
        decimal UnitPrice
    );
}
