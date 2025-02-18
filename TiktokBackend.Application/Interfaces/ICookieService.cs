namespace TiktokBackend.Application.Interfaces
{
    public interface ICookieService
    {
        void SetAccessToken(string token);
        void SetRefreshToken(string token);
    }
}
