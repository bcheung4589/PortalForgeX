using PortalForgeX.Application.Notifiers;

namespace PortalForgeX.Infrastructure.Notifiers;

public sealed class NoOpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage) => Task.CompletedTask;
}
