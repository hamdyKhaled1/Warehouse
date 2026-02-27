using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Warehouses.GetById
{
    public record GetWarehouseByIdQuery(int Id) : IRequest<Result<WarehouseResponse>>;
}
