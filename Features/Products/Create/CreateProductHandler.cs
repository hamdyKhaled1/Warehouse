using MediatR;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.Create
{
    public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, int>
    {
        private readonly WarehouseDbContext _context;

        public CreateProductHandler(WarehouseDbContext context)
        {
            _context = context;
        }

        
        /// Inserts new product 
     
        public async Task<int> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
