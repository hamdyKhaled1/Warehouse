//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Warehouse.Common;
//using Warehouse.Infrastructure.Data;

//namespace Warehouse.Features.OrderItems.Update
//{
//    public class UpdateOrderItemHandler
//        : IRequestHandler<UpdateOrderItemCommand, Result<OrderItemResponse>>
//    {
//        private readonly WarehouseDbContext _context;

//        public UpdateOrderItemHandler(WarehouseDbContext context) => _context = context;

//        public async Task<Result<OrderItemResponse>> Handle(
//            UpdateOrderItemCommand request,
//            CancellationToken cancellationToken)
//        {
//            // 1. OrderItem موجود؟
//            var item = await _context.OrderItems
//                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

//            if (item is null)
//                return Result<OrderItemResponse>.Failure(
//                    $"OrderItem with Id {request.Id} not found.");

//            // 2. جيب الـ Stock
//            var stock = await _context.Stocks
//                .FirstOrDefaultAsync(s =>
//                    s.ProductId == item.ProductId,
//                    cancellationToken);

//            if (stock is null)
//                return Result<OrderItemResponse>.Failure(
//                    $"No stock found for Product with Id {item.ProductId}.");

//            // 3. احسب الفرق في الكمية
//            var quantityDiff = request.Quantity - item.Quantity;

//            // 4. تحقق من الـ Stock لو الكمية زادت
//            if (quantityDiff > 0 && stock.Quantity < quantityDiff)
//                return Result<OrderItemResponse>.Failure(
//                    $"Insufficient stock. Available: {stock.Quantity}, Needed: {quantityDiff}.");

//            // 5. تحديث الـ TotalAmount في الـ Order
//            var order = await _context.Orders
//                .FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);

//            if (order is not null)
//            {
//                // اطرح القديم وضيف الجديد
//                var oldSubtotal = item.Quantity * item.UnitPrice;
//                var newSubtotal = request.Quantity * request.UnitPrice;
//                order.TotalAmount = (order.TotalAmount ?? 0) - oldSubtotal + newSubtotal;
//            }

//            // 6. تحديث الـ Stock
//            stock.Quantity -= quantityDiff;

//            // 7. تحديث الـ OrderItem
//            item.Quantity = request.Quantity;
//            item.UnitPrice = request.UnitPrice;

//            try
//            {
//                await _context.SaveChangesAsync(cancellationToken);
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                return Result<OrderItemResponse>.Failure(
//                    "A conflict occurred while updating. Please try again.");
//            }

//            return Result<OrderItemResponse>.Ok(
//                new OrderItemResponse(
//                    item.Id,
//                    item.OrderId,
//                    item.ProductId,
//                    item.Quantity,
//                    item.UnitPrice),
//                "Order item updated successfully.");
//        }
//    }
//}
