using MediatR;

namespace Warehouse.Features.Products.Create
{
    public record CreateProductCommand(
     string Name,
     string Description,
     decimal Price
 ) : IRequest<int>;
}
