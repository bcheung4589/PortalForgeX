using MimeKit;

namespace PortalForgeX.Application.Notifiers;

/// <summary>
/// Send messages over SMTP.
/// </summary>
public interface ISmtpService
{
    /// <summary>
    /// Create a new Message.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="name"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    MimeMessage NewMessage(string email, string name, string? subject = null);

    /// <summary>
    /// Send the provided message over SMTP.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default);
}
