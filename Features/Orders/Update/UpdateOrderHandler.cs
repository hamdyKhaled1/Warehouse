using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Update
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly WarehouseDbContext _context;

        public UpdateOrderHandler(WarehouseDbContext context)
        {
            _context = context;
        }

        
        /// Updates order status and optionally updates OrderItems.
       
        
        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.Id && o.IsDeleted == false, cancellationToken);

            if (order == null)
                throw new Exception("Order not found or deleted.");

            order.Status = request.Status;

            if (request.UpdatedItems != null)
            {
                // Remove old items
                _context.OrderItems.RemoveRange(order.OrderItems);

                decimal total = 0;

                // Add new items
                foreach (var item in request.UpdatedItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null) throw new Exception($"Product {item.ProductId} not found.");

                    total += product.Price * item.Quantity;

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    });
                }

                order.TotalAmount = total;
            }

            await _context.SaveChangesAsync(cancellationToken);
            
        }
    }
}
