namespace TiktokBackend.Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        Task AddOrSkipUserRoleAsync(Guid userId);
    }
}
