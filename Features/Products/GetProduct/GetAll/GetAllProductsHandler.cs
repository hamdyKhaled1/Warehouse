using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.GetProduct.GetAll
{
    public class GetAllProductsHandler
        : IRequestHandler<GetAllProductsQuery, Result<List<ProductResponse>>>
    {
        private readonly WarehouseDbContext _context;

        public GetAllProductsHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<List<ProductResponse>>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.IsActive))
                .ToListAsync(cancellationToken);

            return Result<List<ProductResponse>>.Ok(
                products, $"{products.Count} products found.");
        }
    }
}
