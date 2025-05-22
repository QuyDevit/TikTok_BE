using Nest;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class VideoSearchService : IVideoSearchService
    {
        private readonly IElasticClient _elasticClient;

        public VideoSearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<(List<VideoDto> Videos, long TotalCount)> GetListVideosAsync(int page, int pageSize, string type)
        {
            var from = (page - 1) * pageSize;

            var searchRequest = new SearchRequest<VideoDto>("videos")
            {
                From = from,
                Size = pageSize,
                Sort = new List<ISort>
                {
                    new FieldSort { Field = "publishedAt", Order = SortOrder.Descending }
                }
            };

            if (type == "trending")
            {
                searchRequest.Query = new QueryContainerDescriptor<VideoDto>()
                    .Range(r => r
                        .Field(f => f.LikesCount)
                        .GreaterThanOrEquals(100) 
                    );
            }

            var response = await _elasticClient.SearchAsync<VideoDto>(searchRequest);

            return (response.Documents.ToList(), response.Total);
        }


        public async Task IndexVideoAsync(VideoDto videoDto)
        {
            await _elasticClient.IndexAsync(videoDto, idx => idx
               .Index("videos")
               .Id(videoDto.Id.ToString())
           );
        }

        public async Task<(List<VideoDto> Videos, long TotalCount)> SearchVideosAsync(string keyword, int page, int pageSize)
        {
            int from = (page - 1) * pageSize;

            var response = await _elasticClient.SearchAsync<VideoDto>(s => s
                .Index("videos")
                .From(from)
                .Size(pageSize)
                .Query(q => q.Bool(b => b
                    .Should(
                        q.Wildcard(m => m
                            .Field(f => f.Description)
                            .Value($"*{keyword.ToLower()}*")
                        ),
                        q.Fuzzy(m => m
                            .Field(f => f.Description)
                            .Value(keyword)
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                    .MinimumShouldMatch(1)
                ))
                .Sort(srt => srt.Descending(f => f.PublishedAt))
            );

            var videos = response.Documents.ToList();
            var total = response.Total;

            return (videos, total);
        }

        public async Task UpdateLikesCountAsync(Guid videoId, int likesCount)
        {
            await _elasticClient.UpdateAsync<VideoDto,object>(videoId.ToString(), u => u
                .Index("videos")
                .Doc(new { likesCount = likesCount })
            );
        }
    }
}
