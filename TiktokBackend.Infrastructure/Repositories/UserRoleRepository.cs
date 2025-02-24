using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;
        public UserRoleRepository(AppDbContext context) {
            _context = context;
        }
        public async Task AddOrSkipUserRoleAsync(Guid userId)
        {
            var userRole = await _context.UserRoles.FindAsync(userId);
            if(userRole == null)
            {
                var newUserRole = new UserRoles
                {
                    RoleId = Guid.Parse("e7139aac-a0b6-4912-93c7-f63199ad33e2"),
                    UserId = userId
                };
                _context.UserRoles.Add(newUserRole);
            }
        }
    }
}
