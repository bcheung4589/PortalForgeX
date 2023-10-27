using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PortalForgeX.Application.Notifiers;
using PortalForgeX.Infrastructure.Configurations;

namespace PortalForgeX.Infrastructure.Notifiers;

/// <inheritdoc/>
public class SmtpService(IOptions<EmailSettingsOptions> settingOptions) : ISmtpService
{
    private readonly EmailSettingsOptions settings = settingOptions.Value;

    /// <inheritdoc/>
    public MimeMessage NewMessage(string email, string name, string? subject = null)
    {
        var message = new MimeMessage(email)
        {
            Sender = new MailboxAddress(name, email),
            Subject = subject ?? string.Empty,
        };

        message.From.Add(new MailboxAddress(settings.FromName, settings.FromEmail));

        return message;
    }

    /// <inheritdoc/>
    public async Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(settings.Host, settings.Port, settings.UseSsl, cancellationToken);

            await client.AuthenticateAsync(settings.Username, settings.Password, cancellationToken);

            await client.SendAsync(FormatOptions.Default, message, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
