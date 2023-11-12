using Microsoft.AspNetCore.Http;
using PortalForgeX.Application.Tenants;

namespace PortalForgeX.Infrastructure.Tenants;

/// <summary>
/// Scan the HttpContext for potential Tenant. If found, load it into the Accessor.
/// </summary>
/// <param name="tenantAccessor"></param>
public class TenantAccessMiddleware(TenantAccessor tenantAccessor) : IMiddleware
{
    private readonly TenantAccessor _tenantAccessor = tenantAccessor;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await _tenantAccessor.LoadFromAsync(context.User);

        await next(context);
    }
}
