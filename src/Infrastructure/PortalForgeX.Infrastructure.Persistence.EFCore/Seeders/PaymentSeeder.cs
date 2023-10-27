using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Domain.Services;
using PortalForgeX.Shared.DTOs;

namespace PortalForgeX.Persistence.EFCore.Seeders;

public sealed class PaymentSeeder : IPaymentSeeder
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbSet<Payment> _table;

    static readonly Random random = new();

    public PaymentSeeder(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _table = ((DomainContext)_unitOfWork.Context).Set<Payment>();
    }

    public async Task<int> ExecuteAsync(int amount = 100, CancellationToken cancellationToken = default)
    {
        var clientIds = await _unitOfWork.ClientRepository.GetAsQuery().Select(x => x.Id).ToArrayAsync(cancellationToken: cancellationToken);
        if (clientIds == null || !clientIds.Any())
        {
            return 0;
        }

        var businessLocations = await _unitOfWork.BusinessLocationRepository.GetAsQuery().Select(x => new { x.Id, x.ClientId }).ToArrayAsync(cancellationToken: cancellationToken);
        var generatedSeeds = new List<Payment>();
        var payMethods = PaymentMethods.AsArray();
        for (int i = 0; i < amount; i++)
        {
            var businessLocation = businessLocations![random.Next(businessLocations.Length)];

            var paymentPeriod = DateTime.UtcNow.AddDays(random.Next(1000) * -1);
            generatedSeeds.Add(new Payment
            {
                Amount = random.Next(100, 250),
                PaymentMethod = payMethods[random.Next(payMethods.Length)],
                PaymentPeriod = paymentPeriod,
                FulfilledDate = paymentPeriod.AddDays(14),
                TransactionId = random.Next(1000, 9999) + Guid.NewGuid().ToString("N")[..6],
                ClientId = businessLocation.ClientId,
                BusinessLocationId = businessLocation.Id,
            });
        }

        using var transaction = _unitOfWork.BeginTransaction();
        await _table.AddRangeAsync(generatedSeeds, cancellationToken);
        int changes = await _unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return changes;
    }
}