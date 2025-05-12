namespace TiktokBackend.Application.Payloads
{
    public class PostVideoRequest
    {
        public Guid UserId { get; set; }
        public byte[] Video { get; set; }
        public byte[] Thumb { get; set; }
        public string Description { get; set; }
        public string Viewable { get; set; }
        public string[] Allows { get; set; }
        public string OriginalFileName { get; set; }

    }
}
