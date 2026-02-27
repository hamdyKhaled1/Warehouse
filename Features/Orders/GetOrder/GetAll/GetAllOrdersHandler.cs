using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Features.OrderItems;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.GetOrder.GetAll
{
    public class GetAllOrdersHandler
         : IRequestHandler<GetAllOrdersQuery, Result<List<OrderResponse>>>
    {
        private readonly WarehouseDbContext _context;

        public GetAllOrdersHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<List<OrderResponse>>> Handle(
            GetAllOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o=>o.OrderItems)
                .ToListAsync(cancellationToken);
            var response = orders.Select(o => new OrderResponse(
            o.Id,
            o.OrderNumber,
            o.TotalAmount,
            o.Status,
            o.CreatedAt,
            o.OrderItems.Select(i => new OrderItemResponse(
                i.Id,
                i.OrderId,
                i.ProductId,
                i.Quantity,
                i.UnitPrice)).ToList()
        )).ToList();

            return Result<List<OrderResponse>>.Ok(
                response, $"{orders.Count} orders found.");
        }
    }
}
