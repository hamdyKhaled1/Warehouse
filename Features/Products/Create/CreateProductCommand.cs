using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Products.Create
{
    public record CreateProductCommand(
          string Name,
          string? Description,
          decimal Price,
          bool IsActive
      ) : IRequest<Result<ProductResponse>>;
}
