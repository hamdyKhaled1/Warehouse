using MediatR;
using Warehouse.Common;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.GetOrder.GetAll
{
    public record GetAllOrdersQuery() : IRequest<Result<List<OrderResponse>>>;
}
