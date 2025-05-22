using Microsoft.EntityFrameworkCore;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly AppDbContext _context;
        public FollowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Guid>> GetFollowedUserIdsAsync(Guid currentUserId, List<Guid> targetUserIds)
        {
            return await _context.Follows
                .Where(f => f.FollowerId == currentUserId && targetUserIds.Contains(f.FolloweeId))
                .Select(f => f.FolloweeId)
                .ToListAsync();
        }

        public async Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId)
        {
            return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        }

        public async Task<bool> ToggleFollowAsync(Guid followerId, Guid followeeId)
        {
            var existingFollow = await _context.Follows.FindAsync(followerId, followeeId);

            if (existingFollow == null)
            {
                _context.Follows.Add(new Follow
                {
                    FollowerId = followerId,
                    FolloweeId = followeeId,
                    FollowedAt = DateTime.UtcNow
                });
                return true;
            }
            else
            {
                _context.Follows.Remove(existingFollow);
                return false;
            }
        }
    }
}
