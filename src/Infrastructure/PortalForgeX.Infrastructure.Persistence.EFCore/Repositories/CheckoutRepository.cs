using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class CheckoutRepository : LoadedRepositoryBase<Checkout, int>, ICheckoutRepository
{
    public CheckoutRepository(DomainContext context) : base(context) { }

    protected override IQueryable<Checkout> IncludedTable()
        => Table // # Default Includes
            .Include(x => x.BusinessLocation);
}
