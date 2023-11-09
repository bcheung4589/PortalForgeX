using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Tenants;

namespace PortalForgeX.Infrastructure.Tenants;

public class TenantAccessMiddleware : IMiddleware
{
    private readonly ITenantService _tenantService;
    private readonly TenantAccessor _tenantAccessor;

    public TenantAccessMiddleware(ITenantService tenantService, TenantAccessor tenantAccessor)
    {
        _tenantService = tenantService;
        _tenantAccessor = tenantAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Retrieve the TenantId Claim
            var tenantClaim = context.User.Claims.FirstOrDefault(x => x.Type == TenantClaimTypes.TenantId);
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                _tenantAccessor.CurrentTenant = await _tenantService.GetByIdAsync(tenantId);
            }

            await next(context);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
