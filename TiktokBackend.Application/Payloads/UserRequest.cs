namespace TiktokBackend.Application.Payloads
{
    public class UserRequest
    {
        public class FollowUserId
        {
            public Guid UserId
            {
                get; set;
            }
        }
    }
}
