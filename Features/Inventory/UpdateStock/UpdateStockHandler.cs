using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Inventory.UpdateStock
{
    public class UpdateStockHandler
     : IRequestHandler<UpdateStockCommand>
    {
        private readonly WarehouseDbContext _context;

        public UpdateStockHandler(WarehouseDbContext context)
        {
            _context = context;
        }

       
        /// Updates stock safely using RowVersion.
        
        public async Task Handle(
            UpdateStockCommand request,
            CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s =>
                    s.ProductId == request.ProductId &&
                    s.WarehouseId == request.WarehouseId,
                    cancellationToken);

            if (stock == null)
                throw new Exception("Stock not found.");

            stock.Quantity = request.Quantity;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Stock modified by another user.");
            }

            
        }
    }
}
