using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Common.JWT;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Account.Login
{

    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly WarehouseDbContext _context;
        private readonly IJwtService _jwtService;

        public LoginHandler(WarehouseDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isValid)
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);

            return Result<LoginResponse>.Ok(
                new LoginResponse(token, user.Email, user.Role),
                "Login successful.");
        }
    }
}
