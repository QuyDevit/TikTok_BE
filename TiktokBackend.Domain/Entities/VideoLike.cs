namespace TiktokBackend.Domain.Entities
{
    public class VideoLike
    {
        public Guid UserId { get; set; }
        public Guid VideoId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
