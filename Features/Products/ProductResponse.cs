namespace Warehouse.Features.Products
{
    public record ProductResponse(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        bool IsActive
    );
}
