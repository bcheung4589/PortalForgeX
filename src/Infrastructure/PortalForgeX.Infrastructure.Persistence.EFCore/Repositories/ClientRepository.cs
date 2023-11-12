using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class ClientRepository : LoadedRepositoryBase<Client, Guid>, IClientRepository
{
    public ClientRepository(DomainContext context) : base(context) { }

    protected override IQueryable<Client> IncludedTable()
        => Table // # Default Includes
            .Include(x => x.Locations!)
                .ThenInclude(x => x.Checkouts)
            .Include(x => x.Contacts)
            .Include(x => x.Payments);
}
