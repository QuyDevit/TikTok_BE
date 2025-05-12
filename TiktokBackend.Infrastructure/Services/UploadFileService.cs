using Microsoft.Extensions.Configuration;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IConfiguration _configuration;
        public UploadFileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadAsync(byte[] fileData, string fileName,string type,string path)
        {
            var uploadPath = Path.Combine("wwwroot", "uploads", type, path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, fileData);

            return $"{_configuration["AppSettings:Domain"]}/uploads/{type}/{path}/{fileName}"; 
        }
    }
}
