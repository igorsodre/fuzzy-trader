using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services.Iterfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FuzzyTrader.Server.Services;

public class EmailClientService : IEmailClientService
{
    private readonly NotificationMetadata _notificationMetadata;

    public EmailClientService(NotificationMetadata notificationMetadata)
    {
        _notificationMetadata = notificationMetadata;
    }

    public async Task<DefaultResult> SendEmailAsync(EmailMessage emailMessage)
    {
        var mimeMessage = CreateMimeMessageFromEmailMessage(emailMessage);

        using var smtpClient = new SmtpClient();
        try
        {
            await smtpClient.ConnectAsync(
                _notificationMetadata.SmtpServer,
                _notificationMetadata.Port,
                SecureSocketOptions.None
            );

            if (_notificationMetadata.UseAuthentication)
            {
                await smtpClient.AuthenticateAsync(
                    _notificationMetadata.UserName,
                    _notificationMetadata.Password
                );
            }

            await smtpClient.SendAsync(mimeMessage);
            await smtpClient.DisconnectAsync(true);
            return new DefaultResult { Success = true };
        }
        catch
        {
            return new DefaultResult { Success = false, ErrorMessages = new[] { "Failed to send email" } };
        }
    }

    private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_notificationMetadata.Sender));
        mimeMessage.To.Add(new MailboxAddress(message.Reciever));
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            { Text = message.Content };
        return mimeMessage;
    }
}