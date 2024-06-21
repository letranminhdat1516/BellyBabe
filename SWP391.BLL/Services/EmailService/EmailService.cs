using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpClient = new SmtpClient(_configuration["EmailSettings:Host"])
        {
            Port = int.Parse(_configuration["EmailSettings:Port"]),
            Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:From"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailsAsync(List<string> emails, string subject, string message)
    {
        var smtpClient = new SmtpClient(_configuration["EmailSettings:Host"])
        {
            Port = int.Parse(_configuration["EmailSettings:Port"]),
            Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:From"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        foreach (var email in emails)
        {
            mailMessage.To.Add(email);
        }

        await smtpClient.SendMailAsync(mailMessage);
    }
}
