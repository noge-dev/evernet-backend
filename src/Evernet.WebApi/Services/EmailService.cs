using System.Net.Mail;
using Evernet.WebApi.Interfaces;

namespace Evernet.WebApi.Services;

public class EmailService : IEmailService
{
    private const string SmtpHost = "localhost";
    private const int SmtpPort = 1025;

    public async Task SendAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(SmtpHost, SmtpPort);
        client.EnableSsl = false;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        var mailMessage = new MailMessage("no-reply@evernet.be", to, subject, body);

        await client.SendMailAsync(mailMessage);
    }
}