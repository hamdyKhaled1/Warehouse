using MediatR;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Warehouses.Create
{
    public class CreateWarehouseHandler
      : IRequestHandler<CreateWarehouseCommand, Result<WarehouseResponse>>
    {
        private readonly WarehouseDbContext _context;

        public CreateWarehouseHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<WarehouseResponse>> Handle(
            CreateWarehouseCommand request,
            CancellationToken cancellationToken)
        {
            var warehouse = new Warehouse.Infrastructure.Data.Warehouse
            {
                Name = request.Name,
                Location = request.Location
            };

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<WarehouseResponse>.Ok(
                new WarehouseResponse(
                    warehouse.Id,
                    warehouse.Name,
                    warehouse.Location),
                "Warehouse created successfully.");
        }
    }
}
