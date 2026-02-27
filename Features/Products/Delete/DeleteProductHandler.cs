using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.Delete
{
    public class DeleteProductHandler
       : IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly WarehouseDbContext _context;

        public DeleteProductHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<bool>> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
                return Result<bool>.Failure(
                    $"Product with Id {request.Id} not found.");

            product.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Ok(true, "Product deleted successfully.");
        }
    }
}
