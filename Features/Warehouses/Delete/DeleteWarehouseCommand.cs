using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Warehouses.Delete
{
    public record DeleteWarehouseCommand(int Id) : IRequest<Result<bool>>;

}
