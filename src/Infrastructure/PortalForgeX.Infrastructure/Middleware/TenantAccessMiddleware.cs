using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Tenants;
using System.Security.Claims;

namespace PortalForgeX.Infrastructure.Middleware;

public class TenantAccessMiddleware(ITenantService tenantService, TenantAccessor tenantAccessor) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Retrieve the TenantId Claim
            var tenantClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GroupSid);
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
