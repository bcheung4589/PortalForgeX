using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Tenants;

namespace PortalForgeX.Infrastructure.Tenants;

public class TenantAccessMiddleware(ITenantService tenantService, TenantAccessor tenantAccessor) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Retrieve the TenantId Claim
            var tenantClaim = context.User.Claims.FirstOrDefault(x => x.Type == TenantClaimTypes.TenantId);
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                tenantAccessor.CurrentTenant = await tenantService.GetByIdAsync(tenantId);
            }

            await next(context);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
