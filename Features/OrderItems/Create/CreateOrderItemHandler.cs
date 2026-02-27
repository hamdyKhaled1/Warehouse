using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.OrderItems.Create
{
    public class CreateOrderItemHandler
       : IRequestHandler<CreateOrderItemCommand, Result<OrderItemResponse>>
    {
        private readonly WarehouseDbContext _context;

        public CreateOrderItemHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<OrderItemResponse>> Handle(
    CreateOrderItemCommand request,
    CancellationToken cancellationToken)
        {
            // 1. Order موجود؟
            var orderExists = await _context.Orders
                .AnyAsync(o => o.Id == request.OrderId, cancellationToken);
            if (!orderExists)
                return Result<OrderItemResponse>.Failure(
                    $"Order with Id {request.OrderId} not found.");

            // 2. Product موجود؟
            var productExists = await _context.Products
                .AnyAsync(p => p.Id == request.ProductId, cancellationToken);
            if (!productExists)
                return Result<OrderItemResponse>.Failure(
                    $"Product with Id {request.ProductId} not found.");

            // 3. جيب الـ Stock
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s =>
                    s.ProductId == request.ProductId,
                    cancellationToken);

            if (stock is null)
                return Result<OrderItemResponse>.Failure(
                    $"No stock found for Product with Id {request.ProductId}.");

            // 4. تحقق من الكمية
            if (stock.Quantity < request.Quantity)
                return Result<OrderItemResponse>.Failure(
                    $"Insufficient stock. Available: {stock.Quantity}, Requested: {request.Quantity}.");

            // 5. إنشاء الـ OrderItem
            var item = new OrderItem
            {
                OrderId = request.OrderId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            _context.OrderItems.Add(item);

            // 6. خصم الكمية
            stock.Quantity -= request.Quantity;

            // 7. تحديث الـ TotalAmount
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order is not null)
                order.TotalAmount = (order.TotalAmount ?? 0) + (request.Quantity * request.UnitPrice);

            // 8. Save - الـ RowVersion بيتحقق تلقائي
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                // ← لو حد تاني اشترى في نفس الوقت
                return Result<OrderItemResponse>.Failure(
                    "Someone else bought this item at the same time. Please try again.");
            }

            return Result<OrderItemResponse>.Ok(
                new OrderItemResponse(
                    item.Id,
                    item.OrderId,
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice),
                "Order item added successfully.");
        }
    }
}
