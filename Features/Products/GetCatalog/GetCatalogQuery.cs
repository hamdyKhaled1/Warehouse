using MediatR;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Products.GetCatalog
{
    public record GetCatalogQuery() : IRequest<List<Product>>;
}
