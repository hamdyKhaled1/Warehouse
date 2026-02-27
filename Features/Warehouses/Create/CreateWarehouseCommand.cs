using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Warehouses.Create
{
    public record CreateWarehouseCommand(
        string Name,
        string? Location
    ) : IRequest<Result<WarehouseResponse>>;
}
