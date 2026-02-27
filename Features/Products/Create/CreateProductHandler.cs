using MediatR;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.Create
{
    public class CreateProductHandler
         : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
    {
        private readonly WarehouseDbContext _context;

        public CreateProductHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<ProductResponse>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                IsActive = request.IsActive
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ProductResponse>.Ok(
                new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.IsActive),
                "Product created successfully.");
        }
    }
}
