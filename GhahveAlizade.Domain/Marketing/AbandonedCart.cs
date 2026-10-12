using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Marketing;

public class AbandonedCart : BaseEntity
{
	public long CartId { get; set; }
	public Commerce.Cart Cart { get; set; } = default!;

	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }

	public decimal CartTotal { get; set; }
	public int ItemCount { get; set; }

	public DateTime DetectedAtUtc { get; set; }
	public DateTime? LastReminderSentAtUtc { get; set; }
	public int ReminderCount { get; set; }

	public bool IsRecovered { get; set; }
	public DateTime? RecoveredAtUtc { get; set; }
	public long? RecoveredOrderId { get; set; }

	public ICollection<AbandonedCartNotification> Notifications { get; set; } = new List<AbandonedCartNotification>();
}