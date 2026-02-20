using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly WarehouseDbContext _context;

        public UpdateProductHandler(WarehouseDbContext context)
        {
            _context = context;
        }

       
        /// Updates product fields: Name, Description, Price, IsActive.
      
      
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsDeleted==false, cancellationToken);

            if (product == null)
                throw new Exception("Product not found or deleted.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
           
        }
    }
}
