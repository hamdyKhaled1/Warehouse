using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Common.JWT;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Account.Login
{

    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public LoginHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValid)
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Result<LoginResponse>.Ok(
                new LoginResponse(token, user.Email!, roles.FirstOrDefault() ?? ""),
                "Login successful.");
        }
    }
}
