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
        Task<bool> AddUserTokenAsync(Guid userId,string rfToken,string ipAddress,string userAgent);
    }
}
