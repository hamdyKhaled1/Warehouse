using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Stocks.GetById
{
    public class GetStockByIdHandler
       : IRequestHandler<GetStockByIdQuery, Result<StockResponse>>
    {
        private readonly WarehouseDbContext _context;

        public GetStockByIdHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<StockResponse>> Handle(
            GetStockByIdQuery request,
            CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks
                .Where(s => s.Id == request.Id)
                .Select(s => new StockResponse(
                    s.Id,
                    s.ProductId,
                    s.WarehouseId,
                    s.Quantity))
                .FirstOrDefaultAsync(cancellationToken);

            if (stock is null)
                return Result<StockResponse>.Failure(
                    $"Stock with Id {request.Id} not found.");

            return Result<StockResponse>.Ok(stock);
        }
    }
}
