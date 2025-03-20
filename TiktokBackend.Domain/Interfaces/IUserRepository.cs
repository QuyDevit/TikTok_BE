using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> ValidateUserCaseEmailAsync(string username,string password);
        Task<User?> ValidateUserCasePhoneAsync(string phone, string password);
        Task<User?> UpdateUserNameByIdAsync(Guid id,string nickname);
        Task<User?> UpdateUserProfileByIdAsync(Guid id,string nickname,string fullname,string bio,string avatar);
        Task<bool> CheckUserNameByIdAsync(Guid id,string nickname);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByNickNameAsync(string nickname);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<List<User>> GetListUserByNameAsync(string query);
    }
}
