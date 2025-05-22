namespace TiktokBackend.Domain.Interfaces
{
    public interface IFollowRepository
    {
        Task<bool> ToggleFollowAsync(Guid followerId, Guid followeeId);

        Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId);
        Task<List<Guid>> GetFollowedUserIdsAsync(Guid currentUserId, List<Guid> targetUserIds);
    }
}
