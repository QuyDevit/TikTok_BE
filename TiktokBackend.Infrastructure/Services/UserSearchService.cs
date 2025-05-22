using Azure;
using Nest;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class UserSearchService : IUserSearchService
    {
        private readonly IElasticClient _elasticClient;

        public UserSearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<(List<UserDto> Users, long TotalCount)> GetListUserSuggestAsync(int page, int pageSize)
        {
            var from = (page - 1) * pageSize;

            var searchRequest = new SearchRequest<UserDto>("users")
            {
                From = from,
                Size = pageSize,
                Sort = new List<ISort>
                {
                    new FieldSort { Field = "followersCount", Order = SortOrder.Descending }
                }
            };

            var response = await _elasticClient.SearchAsync<UserDto>(searchRequest);

            return (response.Documents.ToList(), response.Total);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var response = await _elasticClient.GetAsync<UserDto>(userId.ToString(), idx => idx.Index("users"));
            return response.Found ? response.Source : null;
        }
        public async Task<UserDto> GetUserByNicknameAsync(string nickname)
        {
            var response = await _elasticClient.SearchAsync<UserDto>(s => s
                .Index("users")
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Nickname) 
                        .Query(nickname)
                    )
                )
            );

            return response.Documents.FirstOrDefault(); 
        }

        public async Task IndexUserAsync(UserDto userDto)
        {
            await _elasticClient.IndexAsync(userDto,idx =>idx
                .Index("users")
                .Id(userDto.Id.ToString())
            );
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            await _elasticClient.UpdateAsync<UserDto>(userDto.Id.ToString(), u => u
            .Index("users")
            .Doc(userDto));
        }

        public async Task<(List<UserDto> Users, long TotalCount)> SearchUsersAsync(string keyword, int page, int pageSize)
        {
            int from = (page - 1) * pageSize;

            var response = await _elasticClient.SearchAsync<UserDto>(s => s
                .Index("users")
                .From(from)
                .Size(pageSize)
                .Query(q => q.Bool(b => b
                    .Should(
                                q.Wildcard(m => m
                                    .Field(f => f.Nickname)
                                    .Value($"*{keyword.ToLower()}*")
                                ),
                                q.Wildcard(m => m
                                    .Field(f => f.FullName)
                                    .Value($"*{keyword.ToLower()}*")
                                ),
                                q.Fuzzy(m => m
                                    .Field(f => f.Nickname)
                                    .Value(keyword)
                                    .Fuzziness(Fuzziness.Auto)
                                ),
                                q.Fuzzy(m => m
                                    .Field(f => f.FullName)
                                    .Value(keyword)
                                    .Fuzziness(Fuzziness.Auto)
                                )
                            )
                            .MinimumShouldMatch(1)
                        ))
            );

            var videos = response.Documents.ToList();
            var total = response.Total;

            return (videos, total);
        }
    }
}
