using Microsoft.EntityFrameworkCore.Storage;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Persistence.EFCore.Repositories;

namespace PortalForgeX.Persistence.EFCore;

/// <summary>
/// The Unit of Work is the entrance point for database communication.
/// The UnitOfWork exposes the Database Context and Repositories for each Entity.
/// All the Repositories are registered and maintained in the UoW.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DomainContext _context;

    public IDomainContext Context => _context;

    public IClientRepository ClientRepository { get; }
    public IClientContactRepository ClientContactRepository { get; }
    public IBusinessLocationRepository BusinessLocationRepository { get; }
    public ICheckoutRepository CheckoutRepository { get; }
    public IPaymentRepository PaymentRepository { get; }
    public IUserGroupRepository UserGroupRepository { get; }

    public UnitOfWork(DomainContext context)
    {
        _context = context;

        /// Register the Repositories.
        ClientRepository = new ClientRepository(_context);
        ClientContactRepository = new ClientContactRepository(_context);
        BusinessLocationRepository = new BusinessLocationRepository(_context);
        CheckoutRepository = new CheckoutRepository(_context);
        PaymentRepository = new PaymentRepository(_context);
        UserGroupRepository = new UserGroupRepository(_context);
    }

    public IDbContextTransaction BeginTransaction()
        => _context.Database.BeginTransaction();

    public int SaveChanges()
        => _context.SaveChanges();

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);

    #region [ Dispose ]

    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion [ Dispose ]
}
