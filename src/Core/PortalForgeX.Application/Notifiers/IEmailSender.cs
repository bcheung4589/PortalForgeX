namespace PortalForgeX.Application.Notifiers;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}