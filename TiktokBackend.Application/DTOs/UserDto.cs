namespace TiktokBackend.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; } 
        public string FirstName { get; set; } 

        public string LastName { get; set; } 

        public string FullName { get; set; } 

        public string Nickname { get; set; } 
        public string Avatar { get; set; } 
        public string Email { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Bio { get; set; } 

        public bool Tick { get; set; } 

        public int FollowingsCount { get; set; } 

        public int FollowersCount { get; set; }

        public int LikesCount { get; set; } 

        public string WebsiteUrl { get; set; } 

        public string FacebookUrl { get; set; } 

        public string YoutubeUrl { get; set; } 

        public string TwitterUrl { get; set; } 

        public string InstagramUrl { get; set; } 

    }
}
