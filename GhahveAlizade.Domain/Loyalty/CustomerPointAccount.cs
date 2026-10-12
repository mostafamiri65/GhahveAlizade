using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Loyalty;

public class CustomerPointAccount : BaseEntity
{
	public long CustomerId { get; set; }
	public Customer.Customer Customer { get; set; } = default!;

	public int CurrentPoints { get; set; }
	public int TotalEarnedPoints { get; set; }
	public int TotalRedeemedPoints { get; set; }
	public int ExpiredPoints { get; set; }

	public string? Tier { get; set; } // Bronze, Silver, Gold, Platinum
	public DateTime? TierUpdatedAtUtc { get; set; }

	public ICollection<PointTransaction> Transactions { get; set; } = new List<PointTransaction>();
}