namespace TiktokBackend.Application.Interfaces
{
    public interface IUserContextService
    {
        string GetIpAddress();
        string GetUserAgent();
        string GetDeviceId();
    }
}
