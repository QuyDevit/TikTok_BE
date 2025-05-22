using Microsoft.EntityFrameworkCore;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class VideoLikeRepository : IVideoLikeRepository
    {
        private readonly AppDbContext _context;
        public VideoLikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Guid>> GetLikedVideoIdsByUserAsync(Guid userId, List<Guid> videoIds)
        {
            return await _context.VideoLikes
                .Where(vl => vl.UserId == userId && videoIds.Contains(vl.VideoId))
                .Select(vl => vl.VideoId)
                .ToListAsync();
        }

        public async Task<bool> ToggleLikeAsync(Guid videoId, Guid userId)
        {
            var existingLike = await _context.VideoLikes.FindAsync(userId,videoId);

            if (existingLike == null)
            {
                _context.VideoLikes.Add(new VideoLike
                {
                    VideoId = videoId,
                    UserId = userId,
                    LikedAt = DateTime.UtcNow
                }); 
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                _context.VideoLikes.Remove(existingLike);
                await _context.SaveChangesAsync();
                return false;
            }
        }
    }
}
