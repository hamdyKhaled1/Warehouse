using MediatR;

namespace Warehouse.Features.Orders.Delete
{
    public record DeleteOrderCommand(int Id) : IRequest;
}
