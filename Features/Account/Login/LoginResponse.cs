namespace Warehouse.Features.Account.Login
{
    public record LoginResponse(
         string Token,
         string Email,
         string Role
     );
}
