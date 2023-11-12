using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalForgeX.Application.Tenants;

namespace PortalForgeX.Extensions;

public static class TenantAccessorExtensions
{
    /// <summary>
    /// Load the Tenant; if none found, navigate to dashboard.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="authenticationStateProvider"></param>
    /// <param name="navigation"></param>
    /// <returns></returns>
    public static async Task RequireTenantAsync(this TenantAccessor source, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigation)
    {
        await source.LoadFromAsync((await authenticationStateProvider.GetAuthenticationStateAsync()).User);

        if (!source.HasTenant)
        {
            navigation.NavigateTo("/");
        }
    }
}
