using Microsoft.EntityFrameworkCore;
using Nest;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Services
{
    public class UserSyncService
    {
        private readonly AppDbContext _dbContext;
        private readonly IElasticClient _elasticClient;

        public UserSyncService(AppDbContext dbContext, IElasticClient elasticClient)
        {
            _dbContext = dbContext;
            _elasticClient = elasticClient;
        }

        public async Task SyncUsersToElasticsearch()
        {
            var deleteIndexResponse = await _elasticClient.Indices.DeleteAsync("users");

            var createIndexResponse = await _elasticClient.Indices.CreateAsync("users", c => c
                .Map<UserDto>(m => m.AutoMap())
            );
            var users = await _dbContext.Users
               .Select(u => new UserDto
               {
                   Id = u.Id, 
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   FullName = u.FullName,
                   Nickname = u.Nickname,
                   Avatar = u.Avatar,
                   Email = u.Email,
                   DateOfBirth = u.DateOfBirth,
                   PhoneNumber = u.PhoneNumber,
                   Bio = u.Bio,
                   Tick = u.Tick,
                   FollowingsCount = u.FollowingsCount,
                   FollowersCount = u.FollowersCount,
                   LikesCount = u.LikesCount,
                   WebsiteUrl = u.WebsiteUrl,
                   FacebookUrl = u.FacebookUrl,
                   YoutubeUrl = u.YoutubeUrl,
                   TwitterUrl = u.TwitterUrl,
                   InstagramUrl = u.InstagramUrl
               })
               .ToListAsync();
            var bulkResponse = await _elasticClient.BulkAsync(b => b
               .Index("users")
               .IndexMany(users, (descriptor, user) => descriptor.Id(user.Id.ToString())) 
            );

            if (!bulkResponse.IsValid)
            {
                Console.WriteLine("Error syncing data: " + bulkResponse.DebugInformation);
            }
        }
    }
}
