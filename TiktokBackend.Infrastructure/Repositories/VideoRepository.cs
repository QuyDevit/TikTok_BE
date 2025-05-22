using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly AppDbContext _context;
        public VideoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Video> AddVideoAsync(Video entity)
        {
            await _context.Videos.AddAsync(entity);
            return entity;
        }

        public async Task<int> GetLikeCountFromDbAsync(Guid videoId)
        {
            var currentVideo = await _context.Videos.FindAsync(videoId); 
            return currentVideo != null ? currentVideo.LikesCount : 0;
        }

        public async Task UpdateLikeCountAsync(Guid videoId, int likeCount)
        {
            var currentVideo = await _context.Videos.FindAsync(videoId);
            if (currentVideo != null)
            {
                currentVideo.LikesCount = likeCount;
            }
            _context.SaveChanges();
        }
    }
}
