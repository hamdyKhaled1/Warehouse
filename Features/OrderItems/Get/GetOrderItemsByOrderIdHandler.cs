using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.OrderItems.Get
{
    public class GetOrderItemsByOrderIdHandler
        : IRequestHandler<GetOrderItemsByOrderIdQuery, Result<List<OrderItemResponse>>>
    {
        private readonly WarehouseDbContext _context;

        public GetOrderItemsByOrderIdHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<List<OrderItemResponse>>> Handle(
            GetOrderItemsByOrderIdQuery request,
            CancellationToken cancellationToken)
        {
            // Order موجود؟
            var orderExists = await _context.Orders
                .AnyAsync(o => o.Id == request.OrderId, cancellationToken);

            if (!orderExists)
                return Result<List<OrderItemResponse>>.Failure(
                    $"Order with Id {request.OrderId} not found.");

            var items = await _context.OrderItems
                .Where(i => i.OrderId == request.OrderId)
                .Select(i => new OrderItemResponse(
                    i.Id,
                    i.OrderId,
                    i.ProductId,
                    i.Quantity,
                    i.UnitPrice))
                .ToListAsync(cancellationToken);

            return Result<List<OrderItemResponse>>.Ok(
                items, $"{items.Count} items found.");
        }
    }
}
