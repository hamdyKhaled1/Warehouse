namespace Warehouse.Features.Stocks
{
    public record StockResponse(
       int Id,
       int ProductId,
       int WarehouseId,
       int Quantity
   );
}
