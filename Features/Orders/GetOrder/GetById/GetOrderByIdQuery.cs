using MediatR;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.GetOrder.GetById
{
    public record GetOrderByIdQuery(int Id) : IRequest<OrderDto>;
}
