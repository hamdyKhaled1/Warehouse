using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Orders.Delete
{
    public record DeleteOrderCommand(int Id) : IRequest<Result<bool>>;
}
