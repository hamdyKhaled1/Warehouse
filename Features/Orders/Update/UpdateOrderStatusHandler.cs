using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Features.OrderItems;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Update
{
    public class UpdateOrderStatusHandler
    {
        private readonly WarehouseDbContext _context;

        public UpdateOrderStatusHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<OrderResponse>> Handle(
            UpdateOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order is null)
                return Result<OrderResponse>.Failure(
                    $"Order with Id {request.Id} not found.");

            order.Status = request.NewStatus;
            await _context.SaveChangesAsync(cancellationToken);

            // 6. الرد بنجاح مع تضمين الأصناف
            return Result<OrderResponse>.Ok(
                new OrderResponse(
                    order.Id,
                    order.OrderNumber,
                    order.TotalAmount,
                    order.Status,
                    order.CreatedAt,
                    // تحويل قائمة الأصناف الموجودة في الطلب إلى OrderItemResponse
                    order.OrderItems.Select(i => new OrderItemResponse(
                        i.Id,
                        i.OrderId,
                        i.ProductId,
                        i.Quantity,
                        i.UnitPrice)).ToList()
                ),
                "Order status updated successfully.");
        }
    }
}
