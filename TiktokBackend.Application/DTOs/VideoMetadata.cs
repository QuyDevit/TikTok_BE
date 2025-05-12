namespace TiktokBackend.Application.DTOs
{
    public class VideoMetadata
    {
        public long FileSize { get; set; }
        public string FileFormat { get; set; } = string.Empty;
        public string PlaytimeString { get; set; } = string.Empty;
        public double PlaytimeSeconds { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
    }
}
