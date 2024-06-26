using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendEmailsAsync(List<string> emails, string subject, string message);
}
