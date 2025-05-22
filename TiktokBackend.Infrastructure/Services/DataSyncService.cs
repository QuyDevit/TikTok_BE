using Microsoft.EntityFrameworkCore;
using Nest;
using System.Text.Json;
using TiktokBackend.Application.DTOs;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Services
{
    public class DataSyncService
    {
        private readonly AppDbContext _dbContext;
        private readonly IElasticClient _elasticClient;

        public DataSyncService(AppDbContext dbContext, IElasticClient elasticClient)
        {
            _dbContext = dbContext;
            _elasticClient = elasticClient;
        }

        public async Task SyncUsersAsync()
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
        public async Task SyncVideosAsync()
        {
            await _elasticClient.Indices.DeleteAsync("videos");
            await _elasticClient.Indices.CreateAsync("videos", c => c.Map<VideoDto>(m => m.AutoMap()));

            var videoEntities = await _dbContext.Videos
                .Include(v => v.User)
                .Include(v => v.Meta)
                .ToListAsync(); 

            var videos = videoEntities.Select(v => new VideoDto
            {
                Id = v.Id,
                UserId = v.UserId,
                Type = v.Type,
                ThumbUrl = v.ThumbUrl,
                FileUrl = v.FileUrl,
                Description = v.Description,
                Music = v.Music,
                LikesCount = v.LikesCount,
                CommentsCount = v.CommentsCount,
                SharesCount = v.SharesCount,
                ViewsCount = v.ViewsCount,
                Viewable = v.Viewable,
                Allows = JsonSerializer.Deserialize<string[]>(v.Allows),
                PublishedAt = v.PublishedAt,
                Meta= new VideoMetadata
                {
                    FileSize = v.Meta?.FileSize ?? 0,
                    FileFormat = v.Meta?.FileFormat ?? "",
                    PlaytimeString = v.Meta?.PlaytimeString ?? "",
                    PlaytimeSeconds = v.Meta?.PlaytimeSeconds ?? 0,
                    ResolutionX = v.Meta?.ResolutionX ?? 0,
                    ResolutionY = v.Meta?.ResolutionY ?? 0
                },
                User = new UserDto
                {
                    Id = v.User.Id,
                    FirstName = v.User.FirstName,
                    LastName = v.User.LastName,
                    FullName = v.User.FullName,
                    Nickname = v.User.Nickname,
                    Avatar = $"{v.User.Avatar}?t={DateTime.UtcNow.Ticks}",
                    Tick = v.User.Tick
                }
            }).ToList();

            var response = await _elasticClient.BulkAsync(b => b
                .Index("videos")
                .IndexMany(videos, (d, v) => d.Id(v.Id.ToString()))
            );

            if (!response.IsValid)
                Console.WriteLine("Sync Videos Error: " + response.DebugInformation);
        }
    }
}
