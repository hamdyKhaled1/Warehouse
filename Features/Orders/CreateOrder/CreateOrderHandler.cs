using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.CreateOrder
{
    public class CreateOrderHandler
    : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly WarehouseDbContext _context;

        public CreateOrderHandler(WarehouseDbContext context)
        {
            _context = context;
        }

        
        /// Creates order using optimistic concurrency.
        /// Prevents two users from buying last item simultaneously.
       
        public async Task<int> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            decimal total = 0;

            var order = new Order
            {
                OrderNumber = Guid.NewGuid().ToString(),
                Status = "Completed"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var item in request.Items)
            {
                var stock = await _context.Stocks
                    .FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

                if (stock == null || stock.Quantity < item.Quantity)
                    throw new Exception("Insufficient stock");

                stock.Quantity -= item.Quantity;

                var product = await _context.Products.FindAsync(item.ProductId);

                total += product.Price * item.Quantity;

                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = total;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Stock updated by another user. Try again.");
            }

            return order.Id;
        }
    }
}
