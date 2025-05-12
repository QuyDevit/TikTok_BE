using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class VideoMetaRepository : IVideoMetaRepository
    {
        private readonly AppDbContext _context;
        public VideoMetaRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<VideoMeta> AddVideoMetaAsync(VideoMeta videoMeta)
        {
            await _context.VideoMetas.AddAsync(videoMeta);
            return videoMeta;
        }
    }
}
