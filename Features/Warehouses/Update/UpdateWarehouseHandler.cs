using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Warehouses.Update
{
    public class UpdateWarehouseHandler
         : IRequestHandler<UpdateWarehouseCommand, Result<WarehouseResponse>>
    {
        private readonly WarehouseDbContext _context;

        public UpdateWarehouseHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<WarehouseResponse>> Handle(
            UpdateWarehouseCommand request,
            CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (warehouse is null)
                return Result<WarehouseResponse>.Failure(
                    $"Warehouse with Id {request.Id} not found.");

            warehouse.Name = request.Name;
            warehouse.Location = request.Location;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<WarehouseResponse>.Ok(
                new WarehouseResponse(
                    warehouse.Id,
                    warehouse.Name,
                    warehouse.Location),
                "Warehouse updated successfully.");
        }
    }
}
