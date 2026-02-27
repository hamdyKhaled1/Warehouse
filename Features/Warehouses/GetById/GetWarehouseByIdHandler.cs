using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Warehouses.GetById
{
    public class GetWarehouseByIdHandler
       : IRequestHandler<GetWarehouseByIdQuery, Result<WarehouseResponse>>
    {
        private readonly WarehouseDbContext _context;

        public GetWarehouseByIdHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<WarehouseResponse>> Handle(
            GetWarehouseByIdQuery request,
            CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .Where(w => w.Id == request.Id)
                .Select(w => new WarehouseResponse(
                    w.Id,
                    w.Name,
                    w.Location))
                .FirstOrDefaultAsync(cancellationToken);

            if (warehouse is null)
                return Result<WarehouseResponse>.Failure(
                    $"Warehouse with Id {request.Id} not found.");

            return Result<WarehouseResponse>.Ok(warehouse);
        }
    }
}
