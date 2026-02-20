using MediatR;

namespace Warehouse.Features.Products.Delete
{
    public record DeleteProductCommand(int Id) : IRequest;
}
