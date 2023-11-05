using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Enums;
using PortalForgeX.Shared.Constants;

namespace PortalForgeX.Persistence.EFCore;

/// <summary>
/// Database initializer for Portal Context.
/// Migrates the database, adds the default roles and root admin user.
/// </summary>
public class PortalContextInitializer(
    ILogger<PortalContextInitializer> logger,
    IDbContextFactory<PortalContext> portalContextFactory,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager
    )
{
    private readonly ILogger<PortalContextInitializer> _logger = logger;
    private readonly PortalContext _context = portalContextFactory.CreateDbContext();
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    private const string ADMIN_DEFAULT_PASSWORD = "Test@123";

    /// <summary>
    /// Initialize the PortalContext and Migrate the database.
    /// </summary>
    /// <returns></returns>
    public async Task InitialiseAsync()
    {
        if (!_context.Database.IsSqlServer())
        {
            return;
        }

        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An error occurred while initialising the PortalContext database.");
            throw;
        }
    }

    /// <summary>
    /// Seed the PortalContext with test data.
    /// </summary>
    /// <returns></returns>
    public async Task SeedAsync()
    {
        try
        {
            // seed domain roles
            foreach (var roleName in SystemRolesNames.ToList())
            {
                if (_roleManager.Roles.Any(r => r.Name == roleName))
                {
                    continue;
                }

                await _roleManager.CreateAsync(new ApplicationRole(roleName, RoleType.System));
            }

            // seed admin user
            if (_userManager.Users.All(u => u.UserName != SystemUserAccounts.ADMIN_EMAIL))
            {
                var user = new ApplicationUser
                {
                    UserName = SystemUserAccounts.ADMIN_EMAIL,
                    Email = SystemUserAccounts.ADMIN_EMAIL,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreationTime = DateTime.UtcNow
                };

                await _userManager.CreateAsync(user, ADMIN_DEFAULT_PASSWORD);
                await _userManager.AddToRolesAsync(user, SystemRolesNames.ToList());
            }

            _context.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An error occurred while seeding the PortalContext database.");
            throw;
        }
    }
}
