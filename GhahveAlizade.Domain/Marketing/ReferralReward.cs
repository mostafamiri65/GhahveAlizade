using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Marketing;

public class ReferralReward : BaseEntity
{
	public long ReferralId { get; set; }
	public Referral Referral { get; set; } = default!;

	public long CustomerId { get; set; }
	public Customer.Customer Customer { get; set; } = default!;

	public string RewardType { get; set; } = default!; // Points, Coupon, Cash
	public decimal? RewardValue { get; set; }
	public int? RewardPoints { get; set; }
	public long? CouponId { get; set; }

	public bool IsGranted { get; set; }
	public DateTime? GrantedAtUtc { get; set; }
}