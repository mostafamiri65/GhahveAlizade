using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Marketing;

public class Referral : BaseEntity
{
	public long ReferrerCustomerId { get; set; }
	public Customer.Customer ReferrerCustomer { get; set; } = default!;

	public long? RefereeCustomerId { get; set; }
	public Customer.Customer? RefereeCustomer { get; set; }

	public string ReferralCode { get; set; } = default!;
	public string? RefereeEmail { get; set; }
	public string? RefereePhone { get; set; }

	public string Status { get; set; } = "Pending"; // Pending, Completed, Expired
	public DateTime? CompletedAtUtc { get; set; }

	public ICollection<ReferralReward> Rewards { get; set; } = new List<ReferralReward>();
}