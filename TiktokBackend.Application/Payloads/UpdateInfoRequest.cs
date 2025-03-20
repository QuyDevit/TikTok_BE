namespace TiktokBackend.Application.Payloads
{
    public class UpdateInfoRequest
    {
        public class RequestNickname
        {
            public string Nickname { get; set; }
        }
        public class RequestInfo
        {
            public Guid UserId { get; set; }
            public string Fullname { get; set; }
            public string Nickname { get; set; }
            public string Bio { get; set; }
            public byte[] Avatar { get; set; }
        }
    }
}
