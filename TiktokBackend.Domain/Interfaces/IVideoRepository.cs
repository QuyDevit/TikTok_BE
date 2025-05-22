using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IVideoRepository
    {
        Task<Video> AddVideoAsync(Video video);
        Task<int> GetLikeCountFromDbAsync(Guid videoId);
        Task UpdateLikeCountAsync(Guid videoId,int likeCount);
    }
}
