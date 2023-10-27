using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class BusinessLocationRepository : LoadedRepositoryBase<BusinessLocation, int>, IBusinessLocationRepository
{
    public BusinessLocationRepository(DomainContext context) : base(context) { }

    protected override IQueryable<BusinessLocation> IncludedTable()
        => Table // # Default Includes
            .Include(x => x.Client)
            .Include(x => x.Checkouts);
}
