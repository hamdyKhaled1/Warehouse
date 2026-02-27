using MediatR;
using Warehouse.Common;

namespace Warehouse.Features.Account.Register
{
    // WarehouseStaff بيسجل نفسه
    public record RegisterStaffCommand(
        string Email,
        string Password
    ) : IRequest<Result<string>>;

    // Manager - Admin بس هو اللي يضيفه
    public record RegisterManagerCommand(
        string Email,
        string Password
    ) : IRequest<Result<string>>;
}
