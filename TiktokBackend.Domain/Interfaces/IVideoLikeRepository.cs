namespace TiktokBackend.Domain.Interfaces
{
    public interface IVideoLikeRepository
    {
        Task<bool> ToggleLikeAsync(Guid videoid,Guid userid);
        Task<List<Guid>> GetLikedVideoIdsByUserAsync(Guid userId, List<Guid> videoIds);
    }
}
