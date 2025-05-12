using AutoMapper;
using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Users
{
    public record UpdateUserInfoByIdCommand(UpdateInfoRequest.RequestInfo Data) : IRequest<ServiceResponse<UserDto>>;

    public class UpdateUserInfoByIdCommandHandler : IRequestHandler<UpdateUserInfoByIdCommand, ServiceResponse<UserDto>>
    {
        private readonly IUploadFileService _uploadFileService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserCacheService _userCache;
        private readonly IUserSearchService _userSearchService;

        public UpdateUserInfoByIdCommandHandler(IUploadFileService uploadFileService, IUserRepository userRepository, 
            IUnitOfWork unitOfWork,IMapper mapper,IUserCacheService userCache,IUserSearchService userSearchService)
        {
            _uploadFileService = uploadFileService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userCache = userCache;
            _userSearchService = userSearchService;
        }

        public async Task<ServiceResponse<UserDto>> Handle(UpdateUserInfoByIdCommand request, CancellationToken cancellationToken)
        {
            var rq = request.Data;
            var isExistNickName = await _userRepository.CheckUserNameByIdAsync(rq.UserId, rq.Nickname);
            if (isExistNickName)
            {
                return ServiceResponse<UserDto>.Fail("Tiktok Id đã tồn tại!");
            }
            var avatarUrl = "";
            if (rq.Avatar != null && rq.Avatar.Length > 0)
            {
                string fileName = $"{rq.UserId}.jpeg";
                avatarUrl = await _uploadFileService.UploadAsync(rq.Avatar, fileName, "images", "users");
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _userRepository.UpdateUserProfileByIdAsync(rq.UserId, rq.Nickname,rq.Fullname,rq.Bio, avatarUrl);
                var userDto = _mapper.Map<UserDto>(user);
                await _userSearchService.UpdateUserAsync(userDto);

                await _unitOfWork.CommitAsync();
                await _userCache.RemoveUserAsync(rq.UserId);
                return ServiceResponse<UserDto>.Ok(userDto, "Cập nhật hồ sơ thành công!");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return ServiceResponse<UserDto>.Fail("Cập nhật hồ sơ thất bại!");
            }
        }
    }
}
