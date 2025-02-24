using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<string> GetEmailTemplateAsync(string templateName, Dictionary<string, string> placeholders)
        {
            try
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/templates", $"{templateName}.html");
                if (!File.Exists(templatePath)) {
                    throw new FileNotFoundException($"Không tìm thấy template email: {templatePath}");
                }
                string templateContent = await File.ReadAllTextAsync(templatePath);
                foreach (var placeholder in placeholders)
                {
                    templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
                }
                return templateContent;
            }
            catch (Exception ex) {
                throw new Exception($"Lỗi khi tải email template: {ex.Message}");
            }
        }
    }
}
