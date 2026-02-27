using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Stocks.Update
{
    public record UpdateStockCommand(
      int Id,
      int Quantity
  ) : IRequest<Result<StockResponse>>;
}
