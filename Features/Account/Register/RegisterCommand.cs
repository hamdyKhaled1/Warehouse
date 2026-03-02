using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Account.Register
{
    public record RegisterStaffCommand(
         string Email,
         string Password
     ) : IRequest<Result<string>>;

    public record RegisterManagerCommand(
        string Email,
        string Password
    ) : IRequest<Result<string>>;
}
