using TiktokBackend.Application.DTOs;

namespace TiktokBackend.Application.Interfaces
{
    public interface IVideoSearchService
    {
        Task IndexVideoAsync(VideoDto videoDto);
        Task<(List<VideoDto> Videos, long TotalCount)> SearchVideosAsync(string keyword, int page, int pageSize);
        Task<(List<VideoDto> Videos, long TotalCount)> GetListVideosAsync(int page, int pageSize, string type);
        Task UpdateLikesCountAsync(Guid videoId, int likesCount);
    }
}
