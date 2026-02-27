using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.GetProduct.GetById
{
    public class GetProductByIdHandler
        : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
    {
        private readonly WarehouseDbContext _context;

        public GetProductByIdHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<ProductResponse>> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (product is null)
                return Result<ProductResponse>.Failure(
                    $"Product with Id {request.Id} not found.");

            return Result<ProductResponse>.Ok(product);
        }
    }
}
