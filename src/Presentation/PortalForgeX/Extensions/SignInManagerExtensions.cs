using Microsoft.AspNetCore.Identity;
using PortalForgeX.Domain.Entities.Identity;
using System.Security.Claims;

namespace PortalForgeX.Extensions;

public static class SignInManagerExtensions
{
    public static async Task RegisterSucceededLogin(this SignInManager<ApplicationUser> source, string userName)
    {
        var user = await source.UserManager.FindByNameAsync(userName);
        if (user == null)
        {
            return;
        }

        // Add the TenantId to the User Claims
        await source.UserManager.AddClaimAsync(user, new Claim(ClaimTypes.GroupSid, user.TenantId?.ToString() ?? ""));

        user.LastLoggedInTime = DateTime.UtcNow;
        _ = await source.UserManager.UpdateAsync(user);
    }
}
