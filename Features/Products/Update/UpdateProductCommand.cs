using MediatR;

namespace Warehouse.Features.Products.Update
{
    public record UpdateProductCommand(
     int Id,
     string Name,
     string Description,
     decimal Price,
     bool IsActive
 ) : IRequest;
}
