using MediatR;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.Delete
{
    public class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand>
    {
        private readonly WarehouseDbContext _context;

        public DeleteProductHandler(WarehouseDbContext context)
        {
            _context = context;
        }

       
        /// Marks product as deleted instead of removing it from database.
        
        public async Task Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);

            if (product == null)
                throw new Exception("Product not found.");

            product.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            
        }
    }
}
