using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

/// <summary>
/// The ClientLocation contains information of a businessplace.
/// </summary>
public class BusinessLocation : AuditedEntity<int>
{
    /// <summary>
    /// The API key for the location.
    /// </summary>
    [MaxLength(100)]
    public string ApiKey { get; set; } = null!;

    /// <summary>
    /// The street name of the location.
    /// </summary>
    [MaxLength(200)]
    public string Street { get; set; } = null!;

    /// <summary>
    /// The house number of the location.
    /// </summary>
    [MaxLength(10)]
    public string HouseNr { get; set; } = null!;

    /// <summary>
    /// The zipcode of the location.
    /// </summary>
    [MaxLength(10)]
    public string Zipcode { get; set; } = null!;

    /// <summary>
    /// The place/city in which the location is settled.
    /// </summary>
    [MaxLength(100)]
    public string Place { get; set; } = null!;

    /// <summary>
    /// Country in which the location is settled.
    /// </summary>
    [MaxLength(100)]
    public string Country { get; set; } = null!;

    /// <summary>
    /// Ip-address of the location.
    /// </summary>
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// The startdate of our service.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The enddate of our service.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Indicator if this location has an active subscription.
    /// </summary>
    public bool HasSubscription { get; set; }

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

    /// <summary>
    /// The checkouts at this location.
    /// </summary>
    public IList<Checkout>? Checkouts { get; set; }

    /// <summary>
    /// The payments for this location.
    /// </summary>
    public IList<Payment>? Payments { get; set; }
}
