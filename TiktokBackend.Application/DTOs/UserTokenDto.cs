namespace TiktokBackend.Application.DTOs
{
    public class UserTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}
