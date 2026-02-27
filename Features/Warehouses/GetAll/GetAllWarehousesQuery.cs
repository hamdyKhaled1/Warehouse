using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Warehouses.GetAll
{
    public record GetAllWarehousesQuery() : IRequest<Result<List<WarehouseResponse>>>;
}
