using Warehouse.Features.Account;

namespace Warehouse.Common.JWT
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }

}
