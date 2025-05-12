using AutoMapper;
using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Users
{
    public record UpdateUserNameByIdCommand(Guid UserId,string Nickname) : IRequest<ServiceResponse<UserDto>>;

    public class UpdateUserNameByIdCommandHandler : IRequestHandler<UpdateUserNameByIdCommand, ServiceResponse<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSearchService _userSearchService;
        public UpdateUserNameByIdCommandHandler(IUserRepository userRepository,IMapper mapper,IUnitOfWork unitOfWork,
            IUserSearchService userSearchService) { 
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userSearchService = userSearchService;
        }


        public async Task<ServiceResponse<UserDto>> Handle(UpdateUserNameByIdCommand request, CancellationToken cancellationToken)
        {
            var isExistNickName = await _userRepository.CheckUserNameByIdAsync(request.UserId, request.Nickname);
            if (isExistNickName)
            {
                return ServiceResponse<UserDto>.Fail("Tiktok Id đã tồn tại!");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userRepository.UpdateUserNameByIdAsync(request.UserId, request.Nickname);
                var userDto = _mapper.Map<UserDto>(user);
                await _userSearchService.UpdateUserAsync(userDto);

                await _unitOfWork.CommitAsync();
                return ServiceResponse<UserDto>.Ok(userDto, "Tạo Tiktok id thành công!");
            }
            catch (Exception) { 
                await _unitOfWork.RollbackAsync();
                return ServiceResponse<UserDto>.Fail("Tạo tiktok id thất bại!");
            }
        }
    }
}
