using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Products.Delete
{
    public record DeleteProductCommand(int Id) : IRequest<Result<bool>>;
}
