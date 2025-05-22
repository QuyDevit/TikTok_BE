namespace TiktokBackend.Domain.Entities
{
    public class Follow
    {
        public Guid FollowerId { get; set; } // Người theo dõi
        public virtual User Follower { get; set; }

        public Guid FolloweeId { get; set; }
        public virtual User Followee { get; set; } // Người được theo dõi

        public DateTime FollowedAt { get; set; } = DateTime.Now;
    }

}
