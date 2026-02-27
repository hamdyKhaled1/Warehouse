using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Features.OrderItems;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Update
{

    public class UpdateOrderHandler
       : IRequestHandler<UpdateOrderCommand, Result<OrderResponse>>
    {
        private readonly WarehouseDbContext _context;

        public UpdateOrderHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<OrderResponse>> Handle(
            UpdateOrderCommand request,
            CancellationToken cancellationToken)
        {
            // 1. جلب الأوردر مع الأصناف الحالية
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order is null)
            {
                return Result<OrderResponse>.Failure($"Order with Id {request.Id} not found.");
            }

            // 3. تحديث الأصناف (نمسح القديم ونضيف الجديد)
            _context.OrderItems.RemoveRange(order.OrderItems);
            order.OrderItems = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();
            // 4. إعادة حساب الإجمالي بناءً على الأصناف الجديدة
            order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            await _context.SaveChangesAsync(cancellationToken);

            // 5. الرد بالبيانات الجديدة كاملة
            return Result<OrderResponse>.Ok(new OrderResponse(
                order.Id,
                order.OrderNumber,
                order.TotalAmount,
                order.Status,
                order.CreatedAt,
                order.OrderItems.Select(i => new OrderItemResponse(i.Id, i.OrderId, i.ProductId, i.Quantity, i.UnitPrice)).ToList()
            ), "Order updated successfully.");
        }
              
        }
    }

