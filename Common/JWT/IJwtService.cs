namespace Warehouse.Common.JWT
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, string role);
    }
}
