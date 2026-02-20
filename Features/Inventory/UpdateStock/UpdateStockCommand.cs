using MediatR;

namespace Warehouse.Features.Inventory.UpdateStock
{
    public record UpdateStockCommand(
     int ProductId,
     int WarehouseId,
     int Quantity
 ) : IRequest;
}
