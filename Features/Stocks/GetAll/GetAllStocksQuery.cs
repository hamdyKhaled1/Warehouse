using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Stocks.GetAll
{
    public record GetAllStocksQuery() : IRequest<Result<List<StockResponse>>>;
}

