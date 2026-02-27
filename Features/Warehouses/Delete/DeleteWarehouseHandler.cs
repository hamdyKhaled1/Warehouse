using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Warehouses.Delete
{
    public class DeleteWarehouseHandler
        : IRequestHandler<DeleteWarehouseCommand, Result<bool>>
    {
        private readonly WarehouseDbContext _context;

        public DeleteWarehouseHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<bool>> Handle(
            DeleteWarehouseCommand request,
            CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (warehouse is null)
                return Result<bool>.Failure(
                    $"Warehouse with Id {request.Id} not found.");

            warehouse.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Ok(true, "Warehouse deleted successfully.");
        }
    }
}
