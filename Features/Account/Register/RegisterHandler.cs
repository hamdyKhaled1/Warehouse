using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Account.Register
{
    public class RegisterStaffHandler : IRequestHandler<RegisterStaffCommand, Result<string>>
    {
        private readonly WarehouseDbContext _context;

        public RegisterStaffHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<string>> Handle(
            RegisterStaffCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result<string>.Failure("Email already registered.");

            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "WarehouseStaff"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Ok(user.Email, "Registration successful.");
        }
    }

    public class RegisterManagerHandler : IRequestHandler<RegisterManagerCommand, Result<string>>
    {
        private readonly WarehouseDbContext _context;

        public RegisterManagerHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<string>> Handle(
            RegisterManagerCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result<string>.Failure("Email already registered.");

            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Manager"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Ok(user.Email, "Registration successful.");
        }
    }
}
