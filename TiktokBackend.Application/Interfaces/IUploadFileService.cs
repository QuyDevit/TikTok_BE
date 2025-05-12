namespace TiktokBackend.Application.Interfaces
{
    public interface IUploadFileService
    {
        Task<string> UploadAsync(byte[] fileData, string fileName,string type,string path);
    }
}
