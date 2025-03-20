using Microsoft.Extensions.Configuration;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly string _uploadPath = "wwwroot/uploads/images/users";

        private readonly IConfiguration _configuration;
        public UploadFileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadAsync(byte[] fileData, string fileName)
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            var filePath = Path.Combine(_uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, fileData);

            return $"{_configuration["AppSettings:Domain"]}/uploads/images/users/{fileName}"; 
        }
    }
}
