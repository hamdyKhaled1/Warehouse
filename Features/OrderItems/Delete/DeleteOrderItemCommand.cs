using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.OrderItems.Delete
{
    public record DeleteOrderItemCommand(int Id) : IRequest<Result<bool>>;
}
