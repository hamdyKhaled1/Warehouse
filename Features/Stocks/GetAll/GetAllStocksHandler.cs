using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Stocks.GetAll
{
    public class GetAllStocksHandler
       : IRequestHandler<GetAllStocksQuery, Result<List<StockResponse>>>
    {
        private readonly WarehouseDbContext _context;

        public GetAllStocksHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<List<StockResponse>>> Handle(
            GetAllStocksQuery request,
            CancellationToken cancellationToken)
        {
            var stocks = await _context.Stocks
                .Select(s => new StockResponse(
                    s.Id,
                    s.ProductId,
                    s.WarehouseId,
                    s.Quantity))
                .ToListAsync(cancellationToken);

            return Result<List<StockResponse>>.Ok(
                stocks, $"{stocks.Count} stocks found.");
        }
    }
}
