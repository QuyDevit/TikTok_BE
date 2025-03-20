using TiktokBackend.Application.DTOs;

namespace TiktokBackend.Application.Interfaces
{
    public interface IUserSearchService
    {
        Task IndexUserAsync(UserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task<List<UserDto>> SearchUsersAsync(string keyword);
    }
}
