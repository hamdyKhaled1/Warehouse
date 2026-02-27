using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Stocks.Update
{
    public class UpdateStockHandler
        : IRequestHandler<UpdateStockCommand, Result<StockResponse>>
    {
        private readonly WarehouseDbContext _context;

        public UpdateStockHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<StockResponse>> Handle(
            UpdateStockCommand request,
            CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (stock is null)
                return Result<StockResponse>.Failure(
                    $"Stock with Id {request.Id} not found.");

            stock.Quantity = request.Quantity;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<StockResponse>.Failure(
                    "A conflict occurred while updating. Please try again.");
            }

            return Result<StockResponse>.Ok(
                new StockResponse(
                    stock.Id,
                    stock.ProductId,
                    stock.WarehouseId,
                    stock.Quantity),
                "Stock updated successfully.");
        }
    }
}
