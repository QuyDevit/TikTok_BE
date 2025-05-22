using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Application.Queries.Videos
{
    public record GetListVideoByKeywordQuery(string Keyword, string Type, int Page, int Loadmore) : IRequest<PagedResponse<VideoDto>>;
    public class GetListVideoByKeywordQueryHandler : IRequestHandler<GetListVideoByKeywordQuery, PagedResponse<VideoDto>>
    {
        private readonly IVideoSearchService _videoSearchService;

        public GetListVideoByKeywordQueryHandler(IVideoSearchService videoSearchService)
        {
            _videoSearchService = videoSearchService;
        }

        public async Task<PagedResponse<VideoDto>> Handle(GetListVideoByKeywordQuery request, CancellationToken cancellationToken)
        {
            int pageSize = request.Type == "less" ? 5: 10;

            var (videos, totalRecords) = await _videoSearchService.SearchVideosAsync(request.Keyword, request.Page, pageSize);

            return PagedResponse<VideoDto>.Create(videos, request.Page, pageSize, (int)totalRecords);
        }
    }
}
