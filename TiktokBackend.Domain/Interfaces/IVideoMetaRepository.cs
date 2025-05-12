using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Domain.Interfaces
{
    public interface IVideoMetaRepository
    {
        Task<VideoMeta> AddVideoMetaAsync(VideoMeta videoMeta);
    }
}
