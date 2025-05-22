using TiktokBackend.Application.DTOs;

namespace TiktokBackend.Application.Interfaces
{
    public interface IUserSearchService
    {
        Task IndexUserAsync(UserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task<(List<UserDto> Users, long TotalCount)> SearchUsersAsync(string keyword, int page, int pageSize);
        Task<(List<UserDto> Users, long TotalCount)> GetListUserSuggestAsync(int page, int pageSize);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<UserDto> GetUserByNicknameAsync(string nickname);
    }
}
