using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.Users
{
    public record GetUserByIdQuery(Guid Id) : IRequest<ServiceResponse<UserDto>>;
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ServiceResponse<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);
            if (user is null)
                return ServiceResponse<UserDto>.Fail("Không tìm thấy người dùng!");

            return ServiceResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Lấy thông tin người dùng thành công!");
        }
    }

}
