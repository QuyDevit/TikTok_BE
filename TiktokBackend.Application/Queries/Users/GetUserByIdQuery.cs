using AutoMapper;
using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetUserByIdQuery(Guid Id) : IRequest<ServiceResponse<UserDto>>;
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ServiceResponse<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserCacheService _userCache;

        public GetUserByIdQueryHandler(IMapper mapper, IUserRepository userRepository,IUserCacheService userCache)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userCache = userCache;
        }
        public async Task<ServiceResponse<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedUser = await _userCache.GetUserAsync(request.Id);
            if (cachedUser != null) {
                return ServiceResponse<UserDto>.Ok(cachedUser, "Lấy thông tin người dùng thành công!!");
            }

            var user = await _userRepository.GetUserByIdAsync(request.Id);
            if (user is null)
                return ServiceResponse<UserDto>.Fail("Không tìm thấy người dùng!");

            await _userCache.SetUserAsync(request.Id, _mapper.Map<UserDto>(user), 300);

            return ServiceResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Lấy thông tin người dùng thành công!");
        }
    }

}
