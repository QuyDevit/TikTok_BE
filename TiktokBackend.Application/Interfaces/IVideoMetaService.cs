using TiktokBackend.Application.DTOs;

namespace TiktokBackend.Application.Interfaces
{
    public interface IVideoMetaService
    {
        Task<VideoMetadata> AnalyzeAsync(byte[] videoBytes, string fileName);
    }
}
