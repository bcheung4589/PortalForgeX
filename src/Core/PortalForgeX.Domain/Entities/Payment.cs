using PortalForgeX.Domain.Entities.Internal;
using System.ComponentModel.DataAnnotations;

namespace PortalForgeX.Domain.Entities;

/// <summary>
/// Payments are registered each time a client does an payment.
/// </summary>
public class Payment : AuditedEntity<Guid>
{
    /// <summary>
    /// The amount thats payed.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The method how the payment is fulfilled.
    /// </summary>
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = null!;

    /// <summary>
    /// The period of which the payment is fulfilled.
    /// </summary>
    public DateTime PaymentPeriod { get; set; }

    /// <summary>
    /// The date the payment has been fulfilled.
    /// </summary>
    public DateTime FulfilledDate { get; set; }

    /// <summary>
    /// The transaction id.
    /// </summary>
    [MaxLength(100)]
    public string TransactionId { get; set; } = null!;

    /// <summary>
    /// Remarks about the payment.
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
    /// Client Location reference.
    /// </summary>
    public int? BusinessLocationId { get; set; }

    /// <summary>
    /// Client instance.
    /// </summary>
    public BusinessLocation? BusinessLocation { get; set; }
}
