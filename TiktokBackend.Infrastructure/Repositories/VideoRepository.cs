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
    public class VideoRepository : IVideoRepository
    {
        private readonly AppDbContext _context;
        public VideoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Video> AddVideoAsync(Video entity)
        {
            await _context.Videos.AddAsync(entity);
            return entity;
        }
    }
}
