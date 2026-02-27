using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Stocks.Delete
{
    public class DeleteStockHandler
       : IRequestHandler<DeleteStockCommand, Result<bool>>
    {
        private readonly WarehouseDbContext _context;

        public DeleteStockHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<bool>> Handle(
            DeleteStockCommand request,
            CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (stock is null)
                return Result<bool>.Failure(
                    $"Stock with Id {request.Id} not found.");

            stock.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Ok(true, "Stock deleted successfully.");
        }
    }
}
