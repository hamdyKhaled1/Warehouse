using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Account.Register
{
    public class RegisterStaffHandler
        : IRequestHandler<RegisterStaffCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterStaffHandler(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task<Result<string>> Handle(
            RegisterStaffCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists is not null)
                return Result<string>.Failure("Email already registered.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<string>.Failure(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "WarehouseStaff");

            return Result<string>.Ok(user.Email!, "Registration successful.");
        }
    }

    public class RegisterManagerHandler
        : IRequestHandler<RegisterManagerCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterManagerHandler(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task<Result<string>> Handle(
            RegisterManagerCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists is not null)
                return Result<string>.Failure("Email already registered.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<string>.Failure(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Manager");

            return Result<string>.Ok(user.Email!, "Registration successful.");
        }
    }
}
