using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Warehouses.GetAll
{
    public class GetAllWarehousesHandler
        : IRequestHandler<GetAllWarehousesQuery, Result<List<WarehouseResponse>>>
    {
        private readonly WarehouseDbContext _context;

        public GetAllWarehousesHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<List<WarehouseResponse>>> Handle(
            GetAllWarehousesQuery request,
            CancellationToken cancellationToken)
        {
            var warehouses = await _context.Warehouses
                .Select(w => new WarehouseResponse(
                    w.Id,
                    w.Name,
                    w.Location))
                .ToListAsync(cancellationToken);

            return Result<List<WarehouseResponse>>.Ok(
                warehouses, $"{warehouses.Count} warehouses found.");
        }
    }
}
