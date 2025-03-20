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
        }
    }
}
