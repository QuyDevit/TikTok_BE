using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> ValidateUserAsync(string username,string password);
        Task<User?> UpdateUserNameByIdAsync(Guid id,string userid);
        Task<bool> CheckUserNameByIdAsync(string userid);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<List<User>> GetListUserByNameAsync(string query);
    }
}
