using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly AppDbContext _context;
        public UserTokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUserTokenAsync(Guid userId, string rfToken, string ipAddress, string userAgent)
        {
            var newUserToken = new UserToken
            {
                UserId = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                RefreshToken = rfToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };
            _context.UserTokens.Add(newUserToken);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
