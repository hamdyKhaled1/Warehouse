using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Warehouses.Update
{
    public record UpdateWarehouseCommand(
        int Id,
        string Name,
        string? Location
    ) : IRequest<Result<WarehouseResponse>>;
}
