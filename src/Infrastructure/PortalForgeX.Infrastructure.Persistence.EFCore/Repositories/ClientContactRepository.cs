using Microsoft.EntityFrameworkCore;
using PortalForgeX.Application.Repositories;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Persistence.EFCore;
using PortalForgeX.Persistence.EFCore.Repositories.Internal;

namespace PortalForgeX.Persistence.EFCore.Repositories;

public class ClientContactRepository : LoadedRepositoryBase<ClientContact, Guid>, IClientContactRepository
{
    public ClientContactRepository(DomainContext context) : base(context) { }

    protected override IQueryable<ClientContact> IncludedTable()
        => Table // # Default Includes
            .Include(x => x.Client);
}
