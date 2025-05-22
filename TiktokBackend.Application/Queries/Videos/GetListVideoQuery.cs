using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Queries.Videos
{
    public record GetListVideoQuery(string Type, int Page, int Loadmore, Guid? UserId) : IRequest<PagedResponse<VideoDto>>;
    public class GetListVideoQueryHandler : IRequestHandler<GetListVideoQuery, PagedResponse<VideoDto>>
    {
        private readonly IVideoSearchService _videoSearchService;
        private readonly IVideoLikeCacheService _videoLikeCache;
        private readonly IVideoLikeRepository _videoLikeRepository;
        private readonly IFollowRepository _followRepository;

        public GetListVideoQueryHandler(IVideoSearchService videoSearchService,IVideoLikeCacheService videoLikeCache,
            IVideoLikeRepository videoLikeRepository, IFollowRepository followRepository)
        {
            _videoSearchService = videoSearchService;
            _videoLikeCache = videoLikeCache;
            _videoLikeRepository = videoLikeRepository;
            _followRepository = followRepository;
        }
        public async Task<PagedResponse<VideoDto>> Handle(GetListVideoQuery request, CancellationToken cancellationToken)
        {
            int pageSize = 10;

            var (videos, totalRecords) = await _videoSearchService.GetListVideosAsync(request.Page, pageSize,request.Type);
            var videoIds = videos.Select(v => v.Id).ToList();

            var userIdsInVideos = videos.Select(v => v.User.Id).Distinct().ToList();
            if (request.UserId.HasValue)
            {
                var followedUserIds = await _followRepository.GetFollowedUserIdsAsync(request.UserId.Value, userIdsInVideos);

                foreach (var video in videos)
                {
                    video.User.IsFollowed = followedUserIds.Contains(video.User.Id);
                }
            }
            HashSet<Guid> likedVideoIds = new();
            if (request.UserId.HasValue && videoIds.Any())
            {
                var likedList = await _videoLikeRepository.GetLikedVideoIdsByUserAsync(request.UserId.Value, videoIds);
                likedVideoIds = likedList.ToHashSet();
            }
            foreach (var video in videos)
            {
                var likeInCache = await _videoLikeCache.GetLikeCountAsync(video.Id);
                if (likeInCache.HasValue)
                {
                    video.LikesCount = likeInCache.Value;
                }
                video.IsLiked = likedVideoIds.Contains(video.Id);
            }
            return PagedResponse<VideoDto>.Create(videos, request.Page, pageSize, (int)totalRecords);
        }
    }
}
