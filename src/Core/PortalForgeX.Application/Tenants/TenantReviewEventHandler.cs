using MediatR;
using Microsoft.Extensions.Logging;
using MimeKit;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Notifiers;
using PortalForgeX.Domain.Enums;
using PortalForgeX.Domain.Events;

namespace PortalForgeX.Application.Tenants;

/// <summary>
/// EventHandler to process the reviewing of Tenants 
/// when tenants are approved or rejected.
/// </summary>
/// <param name="logger"></param>
/// <param name="tenantService"></param>
/// <param name="domainContextFactory"></param>
/// <param name="tenantAccessor"></param>
public class TenantReviewEventHandler(
    ILogger<TenantReviewEventHandler> logger,
    ITenantService tenantService,
    IDomainContextFactory domainContextFactory,
    ISmtpService smtpService
    ) :
    INotificationHandler<TenantApprovedEvent>,
    INotificationHandler<TenantRejectedEvent>
{
    private readonly ILogger<TenantReviewEventHandler> _logger = logger;

    /// <summary>
    /// Handle the Approval of given Tenant.
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Handle(TenantApprovedEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Tenant {Name} has been approved on {OccurredDate}.", notification.Tenant.Name, notification.OccurredDate);

        try
        {
            var domainContext = domainContextFactory.CreateDomainContext(notification.Tenant);

            _logger.LogDebug("Tenant Migration Started on {Name}", notification.Tenant.Name);

            await domainContext.MigrateAsync(cancellationToken);

            _logger.LogDebug("Tenant Migration Completed on {Name}", notification.Tenant.Name);

            _ = await tenantService.UpdateStatusAsync(notification.Tenant.Id, TenantStatus.DbMigrated, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Tenant Migration Failed on {Name}", notification.Tenant.Name);
            _logger.LogCritical(ex, ex.Message);
        }

        // send notifications for next phase
        await SendTenantMigratedEmailToManagerAsync(notification, cancellationToken);
    }

    /// <summary>
    /// Send the Tenant Manager a notification that the Tenant is ready for users.
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task SendTenantMigratedEmailToManagerAsync(TenantApprovedEvent notification, CancellationToken cancellationToken = default)
    {
        var tenantManagerEmail = notification.Tenant.Manager?.Email;
        if (tenantManagerEmail is null)
        {
            return;
        }

        try
        {
            var message = smtpService.NewMessage(tenantManagerEmail, "Portal", $"Tenant Migrated: {notification.Tenant.Name}.");
            message.Body = new TextPart()
            {
                Text = $"Tenant {notification.Tenant.Name} ({notification.Tenant.ExternalId}) has been migrated and is ready for Users."
            };
            await smtpService.SendAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed sending notification to {tenantHandlerEmail} [TenantApprovedEvent]", tenantManagerEmail);
            _logger.LogCritical(ex, ex.Message);
        }
    }

    /// <summary>
    /// Handle the Rejection of given Tenant.
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task Handle(TenantRejectedEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Tenant {Name} has been rejected.", notification.Tenant.Name);

        return Task.CompletedTask;
    }
}
