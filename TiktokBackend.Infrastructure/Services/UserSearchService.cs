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

        public async Task IndexUserAsync(UserDto userDto)
        {
            await _elasticClient.IndexAsync(userDto,idx =>idx
                .Index("users")
                .Id(userDto.Id.ToString())
            );
        }

        public async Task<List<UserDto>> SearchUsersAsync(string keyword)
        {
            var response = await _elasticClient.SearchAsync<UserDto>(s => s
                .Index("users")
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
            return response.Documents.ToList();
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            await _elasticClient.UpdateAsync<UserDto>(userDto.Id.ToString(), u => u
            .Index("users")
            .Doc(userDto));
        }
    }
}
