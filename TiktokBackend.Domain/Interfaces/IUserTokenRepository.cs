using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IUserTokenRepository
    {
        Task AddOrUpdateUserTokenAsync(Guid userId,string rfToken,string ipAddress,string userAgent);
        Task<UserToken?> RefreshTokenAsync(string token);
        Task<bool> RemoveRefreshTokenAsync(Guid userId,string token);
    }
}
