using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetUserByNickNameQuery(string Nickname) : IRequest<ServiceResponse<UserDto>>;

    public class GetUserByNickNameQueryHandler : IRequestHandler<GetUserByNickNameQuery, ServiceResponse<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserByNickNameQueryHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<UserDto>> Handle(GetUserByNickNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByNickNameAsync(request.Nickname);
            if (user == null)
                return ServiceResponse<UserDto>.Fail("Không tìm thấy người dùng!");
            return ServiceResponse<UserDto>.Ok(_mapper.Map<UserDto>(user),"Lấy thông tin người dùng thành công!");
        }
    }
}
