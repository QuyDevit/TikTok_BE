namespace TiktokBackend.Application.Payloads
{
    public class VideoRequest
    {
        public class CreateVideo
        {
            public Guid UserId { get; set; }
            public byte[] Video { get; set; }
            public byte[] Thumb { get; set; }
            public string Description { get; set; }
            public string Viewable { get; set; }
            public string[] Allows { get; set; }
            public string OriginalFileName { get; set; }
        }
        public class LikeVideoId
        {
            public Guid VideoId
            {
                get; set;
            }
        } 

    }
}
