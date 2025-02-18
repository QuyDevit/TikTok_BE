using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(Guid id,User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<List<User>> GetListUserByNameAsync(string query);
    }
}
