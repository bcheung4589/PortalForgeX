using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class PaymentRepository : LoadedRepositoryBase<Payment, Guid>, IPaymentRepository
{
    public PaymentRepository(DomainContext context) : base(context) { }

    protected override IQueryable<Payment> IncludedTable()
        => Table // # Default Includes
            .Include(x => x.Client)
            .Include(x => x.BusinessLocation);
}
