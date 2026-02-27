using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Stocks.GetById
{
    public record GetStockByIdQuery(int Id) : IRequest<Result<StockResponse>>;
}
