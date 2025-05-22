using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Videos
{
    public record LikeVideoCommand(Guid VideoId,Guid UserId):IRequest<ServiceResponse<int>>;
    public class LikeVideoCommandHandler : IRequestHandler<LikeVideoCommand, ServiceResponse<int>>
    {
        private readonly IVideoLikeRepository _videoLikeRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoLikeCacheService _videoLikeCache;
        public LikeVideoCommandHandler(IVideoLikeRepository videoLikeRepository, IVideoRepository videoRepository,
            IVideoLikeCacheService videoLikeCache)
        {
            _videoLikeRepository = videoLikeRepository;
            _videoRepository = videoRepository;
            _videoLikeCache = videoLikeCache;
        }

        public async Task<ServiceResponse<int>> Handle(LikeVideoCommand request, CancellationToken cancellationToken)
        {
            bool isLiked = await _videoLikeRepository.ToggleLikeAsync(request.VideoId, request.UserId);

            int newLikeCount;

            if (isLiked)
            {
                newLikeCount = await _videoLikeCache.IncrementLikeAsync(request.VideoId, async () =>
                {
                    return await _videoRepository.GetLikeCountFromDbAsync(request.VideoId);
                });

                return ServiceResponse<int>.Ok(newLikeCount, "Đã like video");
            }
            else
            {
                newLikeCount = await _videoLikeCache.DecrementLikeAsync(request.VideoId, async () =>
                {
                    return await _videoRepository.GetLikeCountFromDbAsync(request.VideoId);
                });

                return ServiceResponse<int>.Ok(newLikeCount, "Đã bỏ like video");
            }
        }
    }
}
