using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalForgeX.Infrastructure.Tenants;
using System.Security.Claims;

namespace PortalForgeX.Client.Authentication;

public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> _unauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!persistentState.TryTakeFromJson<UserContext>(nameof(UserContext), out var userContext) || userContext is null)
        {
            return _unauthenticatedTask;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userContext.UserId),
            new Claim(ClaimTypes.Name, userContext.Email),
            new Claim(ClaimTypes.Email, userContext.Email),
            new Claim(TenantClaimTypes.TenantId, userContext.TenantId ?? "")];

        return Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                authenticationType: nameof(PersistentAuthenticationStateProvider)))));
    }
}
