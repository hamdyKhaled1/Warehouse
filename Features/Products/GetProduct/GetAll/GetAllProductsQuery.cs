using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Products.GetProduct.GetAll
{
    public record GetAllProductsQuery() : IRequest<Result<List<ProductResponse>>>;
}
