using Microsoft.EntityFrameworkCore;
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
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly AppDbContext _context;
        public ApiKeyRepository(AppDbContext context) { 
            _context = context; 
        }
        public async Task<ApiKey?> AddAsync(Guid userId)
        {

            var apiKey = _context.ApiKeys.SingleOrDefault(n => n.UserId == userId);
            if (apiKey is not null) {
                return null;
            }
            ApiKey newApiKey = new ApiKey { 
                UserId = userId,
                Key = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddYears(100) 
            }; 
            _context.ApiKeys.Add(newApiKey);
            await _context.SaveChangesAsync();
            return newApiKey;
        }

        public async Task<bool> GetByKeyAsync(string key)
        {
            var result = await _context.ApiKeys.SingleOrDefaultAsync(x => x.Key == key);
            return result is not null && result.ExpiryDate > DateTime.UtcNow;
        }
    }
}
