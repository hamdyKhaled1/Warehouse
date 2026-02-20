using MediatR;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Delete
{
    public class DeleteOrderHandler
    : IRequestHandler<DeleteOrderCommand>
    {
        private readonly WarehouseDbContext _context;

        public DeleteOrderHandler(WarehouseDbContext context)
        {
            _context = context;
        }

        
        /// Marks order as deleted.
      
        public async Task Handle(
            DeleteOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(request.Id);

            if (order == null)
                throw new Exception("Order not found.");

            order.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

           
        }
    }

}
