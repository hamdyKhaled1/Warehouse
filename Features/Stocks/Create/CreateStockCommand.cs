using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Stocks.Create
{
    public record CreateStockCommand(
        int ProductId,
        int WarehouseId,
        int Quantity
    ) : IRequest<Result<StockResponse>>;
}
