using Microsoft.AspNetCore.Identity;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Domain.Services;
using PortalForgeX.Persistence.EFCore.Seeders.Internals;
using PortalForgeX.Shared.Extensions;

namespace PortalForgeX.Persistence.EFCore.Seeders;

public sealed class TenantSeeder(IPortalContext portalContext, UserManager<ApplicationUser> userManager) : ITenantSeeder
{
    private readonly IPortalContext _portalContext = portalContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly List<string> _generatedNames = [];

    static readonly Random random = new();

    public async Task<int> ExecuteAsync(int amount = 100, CancellationToken cancellationToken = default)
    {
        var generatedSeeds = new List<Tenant>();
        var userIds = _userManager.Users.Select(x => x.Id).ToList();
        for (int i = 0; i < amount; i++)
        {
            var uniqueName = GenerateUniqueName();
            var creationDate = DateTime.UtcNow.AddDays(random.Next(1000) * -1);

            generatedSeeds.Add(new Tenant
            {
                Name = uniqueName,
                CreationTime = creationDate,
                ExternalId = NextStrings(AlphaChars, 10, 10),
                InternalName = uniqueName.SanitizeAlphaNum(),
                Status = Domain.Enums.TenantStatus.Created,
                Host = $"{uniqueName.SanitizeAlphaNum()}.portal.domain.com",
                IsActive = true,
                ManagerId = userIds.OrderBy(x => random.Next()).FirstOrDefault(),
                TenantSettings = new TenantSettings
                {
                    Brand = uniqueName,
                    IsPublicRegisterEnabled = random.Next(2) != 0,
                }
            });
        }

        await _portalContext.Tenants.AddRangeAsync(generatedSeeds, cancellationToken);
        int changes = await _portalContext.SaveChangesAsync(cancellationToken);

        return changes;
    }

    private string GenerateUniqueName()
    {
        string clientName;
        do
        {
            clientName = FictionalNameGenerator.GenerateRandomName(random);
        } while (_generatedNames.Contains(clientName));

        _generatedNames.Add(clientName);
        return clientName;
    }

    private readonly string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string NextStrings(string allowedChars, int minimalValue, int maximalValue)
    {
        var stringLength = random.Next(minimalValue, maximalValue + 1);
        var chars = new char[stringLength];
        for (int i = 0; i < stringLength; ++i)
        {
            chars[i] = allowedChars[random.Next(allowedChars.Length)];
        }

        return new string(chars);
    }
}
