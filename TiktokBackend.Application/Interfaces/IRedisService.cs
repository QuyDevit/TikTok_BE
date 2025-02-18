using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IRedisService
    {
        Task SetAsync(string key, string value, int expirationSeconds);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
    }
}
