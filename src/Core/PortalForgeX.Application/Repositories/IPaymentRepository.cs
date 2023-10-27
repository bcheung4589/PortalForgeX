using PortalForgeX.Application.Repositories.Internal;
using PortalForgeX.Domain.Entities;

namespace PortalForgeX.Application.Repositories;

public interface IPaymentRepository : IRepository<Payment, Guid>
{
}
