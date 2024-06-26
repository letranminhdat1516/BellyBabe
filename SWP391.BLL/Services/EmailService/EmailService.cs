<<<<<<< HEAD
﻿using System.Net;
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
=======
﻿using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _from;

    public EmailService(IConfiguration configuration)
    {
        _host = configuration["EmailSettings:Host"];
        _port = int.Parse(configuration["EmailSettings:Port"]);
        _username = configuration["EmailSettings:Username"];
        _password = configuration["EmailSettings:Password"];
        _from = configuration["EmailSettings:From"];
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using (var client = new SmtpClient(_host, _port)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = true
        })
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);
            await client.SendMailAsync(mailMessage);
        }
>>>>>>> master
    }
}
