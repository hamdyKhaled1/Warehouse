using MediatR;
using Warehouse.Common;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.GetOrder.GetById
{
    public record GetOrderByIdQuery(int Id) : IRequest<Result<OrderResponse>>;
}
