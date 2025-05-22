namespace TiktokBackend.Application.Interfaces
{
    public interface IVideoLikeCacheService
    {
        Task<int> IncrementLikeAsync(Guid videoId, Func<Task<int>> getLikeFromDb);
        Task<int> DecrementLikeAsync(Guid videoId, Func<Task<int>> getLikeFromDb);
        Task<int?> GetLikeCountAsync(Guid videoId);
        Task ResetLikeCountAsync(Guid videoId);
        Task<List<Guid>> GetAllCachedVideoIdsAsync();
    }
}
