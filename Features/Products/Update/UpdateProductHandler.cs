using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.Update
{
    public class UpdateProductHandler
       : IRequestHandler<UpdateProductCommand, Result<ProductResponse>>
    {
        private readonly WarehouseDbContext _context;

        public UpdateProductHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<ProductResponse>> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
                return Result<ProductResponse>.Failure(
                    $"Product with Id {request.Id} not found.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<ProductResponse>.Ok(
                new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.IsActive),
                "Product updated successfully.");
        }
    }
}
