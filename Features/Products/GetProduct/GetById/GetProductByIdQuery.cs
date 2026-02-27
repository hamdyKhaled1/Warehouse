using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Products.GetProduct.GetById
{
    public record GetProductByIdQuery(int Id) : IRequest<Result<ProductResponse>>;
}
