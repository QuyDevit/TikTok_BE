namespace TiktokBackend.Application.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<string> GetEmailTemplateAsync(string templateName, Dictionary<string, string> placeholders);
    }
}
