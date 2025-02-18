using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using TiktokBackend.Application.Interfaces;

namespace TiktokBackend.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Email:SMTPHost"])
                {
                    Port = int.Parse(_configuration["Email:SMTPPort"]),
                    Credentials = new NetworkCredential(
                        _configuration["Email:SMTPUser"],
                        _configuration["Email:SMTPPassword"]
                    ),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:SMTPUser"],"Tiktok Clone"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi chung: {ex.Message}"); 
                return false;
            }
        }
    }
}
