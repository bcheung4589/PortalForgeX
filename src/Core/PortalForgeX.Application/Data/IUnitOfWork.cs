using Microsoft.EntityFrameworkCore.Storage;
using PortalForgeX.Application.Repositories;

namespace PortalForgeX.Application.Data;

public interface IUnitOfWork : IDisposable
{
    IDomainContext Context { get; }

    IClientRepository ClientRepository { get; }
    IClientContactRepository ClientContactRepository { get; }
    IBusinessLocationRepository BusinessLocationRepository { get; }
    ICheckoutRepository CheckoutRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }

    IDbContextTransaction BeginTransaction();

    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
