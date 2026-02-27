using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.Delete
{
    public class DeleteOrderHandler
       : IRequestHandler<DeleteOrderCommand, Result<bool>>
    {
        private readonly WarehouseDbContext _context;

        public DeleteOrderHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<bool>> Handle(
            DeleteOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order is null)
                return Result<bool>.Failure(
                    $"Order with Id {request.Id} not found.");

            order.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Ok(true, "Order deleted successfully.");
        }
    }

}
