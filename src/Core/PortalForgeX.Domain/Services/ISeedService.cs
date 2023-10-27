namespace PortalForgeX.Domain.Services;

/// <summary>
/// Seed the database with Entities that has fake data.
/// </summary>
public interface ISeedService
{
    Task<int> ExecuteAsync(int amount = 100, CancellationToken cancellationToken = default);
}

/// <summary>
/// Seed the database with Payments data.
/// </summary>
public interface IPaymentSeeder : ISeedService { }

/// <summary>
/// Seed the database with Clients data.
/// </summary>
public interface IClientSeeder : ISeedService { }
