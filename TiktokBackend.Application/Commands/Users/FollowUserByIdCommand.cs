using AutoMapper;
using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Users
{
    public record FollowUserByIdCommand(Guid FollowerId,Guid FolloweeId):IRequest<ServiceResponse<bool>>;
    public class FollowUserByIdCommandHandler : IRequestHandler<FollowUserByIdCommand, ServiceResponse<bool>>
    {
        private readonly IFollowRepository _followRepository;
        private readonly IUserSearchService _userSearchService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FollowUserByIdCommandHandler(IFollowRepository followRepository, IUserSearchService userSearchService
            ,IUserRepository userRepository,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _followRepository = followRepository;
            _userSearchService = userSearchService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> Handle(FollowUserByIdCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isFollow = await _followRepository.ToggleFollowAsync(request.FollowerId, request.FolloweeId);
                var followee = await _userRepository.UpdateFollowingsCount(request.FollowerId,isFollow);
                var follower = await _userRepository.UpdateFollowersCount(request.FolloweeId, isFollow);

                await _userSearchService.UpdateUserAsync(_mapper.Map<UserDto>(followee));
                await _userSearchService.UpdateUserAsync(_mapper.Map<UserDto>(follower));

                await _unitOfWork.CommitAsync();
                return ServiceResponse<bool>.Ok(true, isFollow ? "Follow thành công!" : "Đã hủy Follow!");
            }
            catch (Exception ex) {
                return ServiceResponse<bool>.Fail("Follow không thành công");
            }
        
        }
    }
}
