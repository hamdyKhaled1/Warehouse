using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.GetCatalog
{
    public class GetCatalogHandler
    : IRequestHandler<GetCatalogQuery, List<Product>>
    {
        private readonly WarehouseDbContext _context;

        public GetCatalogHandler(WarehouseDbContext context)
        {
            _context = context;
        }

        
        /// Returns products marked as active.
        
        public async Task<List<Product>> Handle(
            GetCatalogQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(p => p.IsActive==true && p.IsDeleted == false)
                .ToListAsync(cancellationToken);
        }
    }
}
