using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

public class Client : AuditedEntity<Guid>
{
    /// <summary>
    /// The name of the client.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Indicator if the client has the customer care plus pack.
    /// </summary>
    public bool HasCustomerCarePlus { get; set; }

    /// <summary>
    /// Indicator if the client is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Remarks about the client.
    /// </summary>
    [MaxLength(2000)]
    public string? Remarks { get; set; }

    /** 
	 * relations 
	 */

    /// <summary>
    /// The contactpersons of this client.
    /// </summary>
    public IList<ClientContact>? Contacts { get; set; }

    /// <summary>
    /// The business locations of this client.
    /// </summary>
    public IList<BusinessLocation>? Locations { get; set; }

    /// <summary>
    /// The payments done by this client.
    /// </summary>
    public IList<Payment>? Payments { get; set; }
}
