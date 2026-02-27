using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Account.Login
{
    public record LoginCommand(
         string Email,
         string Password
     ) : IRequest<Result<LoginResponse>>;
}
