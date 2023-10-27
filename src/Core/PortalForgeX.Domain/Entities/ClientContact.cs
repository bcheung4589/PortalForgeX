using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

public class ClientContact : AuditedEntity<Guid>
{
    /// <summary>
    /// The fullname of the clients contactperson.
    /// </summary>
    [MaxLength(100)]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// The phonenumber of the clients contactperson.
    /// </summary>
    [MaxLength(20)]
    public string PhoneNr { get; set; } = null!;

    /// <summary>
    /// The email of the clients contactperson.
    /// </summary>
    [MaxLength(150)]
    public string? Email { get; set; }

    /// <summary>
    /// Indicator if the clients contactperson is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Remarks about the clients contactperson.
    /// </summary>
    [MaxLength(2000)]
    public string? Remarks { get; set; }

    /** 
	 * relations 
	 */

    /// <summary>
    /// Client reference.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Client instance.
    /// </summary>
    public Client Client { get; set; } = null!;
}
