using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

public class Checkout : AuditedEntity<int>
{
    /// <summary>
    /// The device code for the Checkout instance.
    /// </summary>
    [MaxLength(50)]
    public string DeviceCode { get; set; } = null!;

    /// <summary>
    /// The software version thats installed on the machine.
    /// </summary>
    [MaxLength(10)]
    public string SoftwareVersion { get; set; } = null!;

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
    /// Client Location reference.
    /// </summary>
    public int BusinessLocationId { get; set; }

    /// <summary>
    /// Client Location instance.
    /// </summary>
    public BusinessLocation BusinessLocation { get; set; } = null!;
}
