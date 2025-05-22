using AutoMapper;
using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetUserByNickNameQuery(string Nickname,Guid? CurrentId) : IRequest<ServiceResponse<UserDto>>;

    public class GetUserByNickNameQueryHandler : IRequestHandler<GetUserByNickNameQuery, ServiceResponse<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserSearchService _userSearchService;
        private readonly IFollowRepository _followRepository;

        public GetUserByNickNameQueryHandler(IMapper mapper, IUserSearchService userSearchService, IFollowRepository followRepository)
        {
            _mapper = mapper;
            _followRepository = followRepository;
            _userSearchService = userSearchService;
        }

        public async Task<ServiceResponse<UserDto>> Handle(GetUserByNickNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userSearchService.GetUserByNicknameAsync(request.Nickname);
            if (user == null)
                return ServiceResponse<UserDto>.Fail("Không tìm thấy người dùng!");
            bool isFollowed = false;
            if (request.CurrentId.HasValue)
            {
                isFollowed = await _followRepository.IsFollowingAsync(request.CurrentId.Value, user.Id);
            }
            var userDto = _mapper.Map<UserDto>(user);
            userDto.IsFollowed = isFollowed;
            return ServiceResponse<UserDto>.Ok(userDto, "Lấy thông tin người dùng thành công!");
        }
    }
}
