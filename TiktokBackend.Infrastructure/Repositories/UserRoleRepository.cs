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
        public async Task<bool> AddUserRoleAsync(Guid userId)
        {
            var newUserRole = new UserRoles
            {
                RoleId = Guid.Parse("e7139aac-a0b6-4912-93c7-f63199ad33e2"),
                UserId = userId
            };
            _context.UserRoles.Add(newUserRole);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
