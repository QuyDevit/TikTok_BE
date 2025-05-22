using Microsoft.EntityFrameworkCore;
using TiktokBackend.Domain.Entities;


namespace TiktokBackend.Infrastructure.Persistence
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoMeta> VideoMetas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<VideoLike> VideoLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserToken>()
                .HasIndex(ut => ut.UserId)
                .HasDatabaseName("IX_UserToken_UserId");

            modelBuilder.Entity<UserToken>()
                .HasIndex(ut => ut.RefreshToken)
                .IsUnique()
                .HasDatabaseName("IX_UserToken_RefreshToken");
            modelBuilder.Entity<ApiKey>()
                .HasIndex(ut => ut.UserId)
                .HasDatabaseName("IX_ApiKey_UserId");
            modelBuilder.Entity<ApiKey>()
                .HasIndex(ut => ut.Key)
                .HasDatabaseName("IX_ApiKey_Key");

            modelBuilder.Entity<Follow>()
            .HasKey(f => new { f.FollowerId, f.FolloweeId });

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany()
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany()
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommentLike>()
            .HasKey(f => new { f.UserId, f.CommentId });

            modelBuilder.Entity<VideoLike>()
            .HasKey(f => new { f.UserId, f.VideoId });
        }
    }
}
