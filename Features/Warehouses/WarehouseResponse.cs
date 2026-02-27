namespace Warehouse.Features.Warehouses
{
    public record WarehouseResponse(
       int Id,
       string Name,
       string? Location
   );
}
