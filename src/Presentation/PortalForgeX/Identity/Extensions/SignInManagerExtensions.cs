using Microsoft.AspNetCore.Identity;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Infrastructure.Tenants;
using System.Security.Claims;

namespace PortalForgeX.Identity.Extensions;

public static class SignInManagerExtensions
{
    public static async Task RegisterSucceededLoginAsync(this SignInManager<ApplicationUser> source, string userName)
    {
        var user = await source.UserManager.FindByNameAsync(userName);
        if (user == null)
        {
            return;
        }

        // Add the TenantId to the User Claims
        if (user.TenantId is not null && user.TenantId != Guid.Empty)
        {
            await source.UserManager.AddClaimAsync(user, new Claim(TenantClaimTypes.TenantId, user.TenantId.ToString()!));
        }

        user.LastLoggedInTime = DateTime.UtcNow;
        _ = await source.UserManager.UpdateAsync(user);
    }
}
