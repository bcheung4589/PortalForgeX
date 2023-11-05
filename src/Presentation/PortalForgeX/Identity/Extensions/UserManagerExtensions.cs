using Microsoft.AspNetCore.Identity;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Infrastructure.Tenants;
using System.Security.Claims;

namespace PortalForgeX.Identity.Extensions;

public static class UserManagerExtensions
{
    public static async Task RegisterSucceededLoginAsync(this UserManager<ApplicationUser> source, string userName)
    {
        var user = await source.FindByNameAsync(userName);
        if (user == null)
        {
            return;
        }

        // Assign the TenantId to the User Claims
        if (user.TenantId is not null && user.TenantId != Guid.Empty)
        {
            var userClaims = await source.GetClaimsAsync(user);
            if (userClaims.Any())
            {
                var tenantClaims = userClaims.Where(x => x.Type == TenantClaimTypes.TenantId);
                if (tenantClaims.Any())
                {
                    await source.RemoveClaimsAsync(user, tenantClaims);
                }
            }

            await source.AddClaimAsync(user, new Claim(TenantClaimTypes.TenantId, user.TenantId.ToString()!));
        }

        user.LastLoggedInTime = DateTime.UtcNow;
        _ = await source.UpdateAsync(user);
    }

    public static async Task<IdentityResult> UpdateRolesAsync(this UserManager<ApplicationUser> source, ApplicationUser user, IDictionary<ApplicationRole, bool> userRoles)
    {
        var updatedRole = false;

        var removeFromRoles = userRoles.Where(x => !x.Value).Select(x => x.Key.Name!);
        foreach (var removeFromRole in removeFromRoles)
        {
            if (!(await source.IsInRoleAsync(user, removeFromRole)))
            {
                continue;
            }

            var removeResult = await source.RemoveFromRoleAsync(user, removeFromRole);
            if (!removeResult.Succeeded)
            {
                return removeResult;
            }

            updatedRole = true;
        }

        var addToRoles = userRoles.Where(x => x.Value).Select(x => x.Key.Name!);
        foreach (var addToRole in addToRoles)
        {
            if (await source.IsInRoleAsync(user, addToRole))
            {
                continue;
            }

            var addResult = await source.AddToRoleAsync(user, addToRole);
            if (!addResult.Succeeded)
            {
                return addResult;
            }

            updatedRole = true;
        }

        /// Trigger the LastModificationTime for role updates.
        if (updatedRole)
        {
            user.LastModificationTime = DateTime.UtcNow;
            _ = await source.UpdateAsync(user);
        }

        return IdentityResult.Success;
    }

    public static async Task<IdentityResult> UpdatePasswordAsync(this UserManager<ApplicationUser> source, ApplicationUser user, string changedPassword)
    {
        var resetToken = await source.GeneratePasswordResetTokenAsync(user);
        return await source.ResetPasswordAsync(user, resetToken, changedPassword);
    }
}
