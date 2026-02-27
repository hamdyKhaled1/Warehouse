using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.OrderItems.Delete
{
    public class DeleteOrderItemHandler
         : IRequestHandler<DeleteOrderItemCommand, Result<bool>>
    {
        private readonly WarehouseDbContext _context;

        public DeleteOrderItemHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<bool>> Handle(
            DeleteOrderItemCommand request,
            CancellationToken cancellationToken)
        {
            var item = await _context.OrderItems
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item is null)
                return Result<bool>.Failure(
                    $"OrderItem with Id {request.Id} not found.");

            // رجع الكمية للـ Stock
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s =>
                    s.ProductId == item.ProductId,
                    cancellationToken);

            if (stock is not null)
                stock.Quantity += item.Quantity;

            // تحديث الـ TotalAmount في الـ Order
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);

            if (order is not null)
                order.TotalAmount = (order.TotalAmount ?? 0) - (item.Quantity * item.UnitPrice);

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Ok(true, "Order item deleted successfully.");
        }
    }
}


