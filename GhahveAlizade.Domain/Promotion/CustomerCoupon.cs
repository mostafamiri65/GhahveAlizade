using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Promotion;

public class CustomerCoupon : BaseEntity
{
	public long CustomerId { get; set; }
	public Customer.Customer Customer { get; set; } = default!;

	public long CouponId { get; set; }
	public Coupon Coupon { get; set; } = default!;

	public bool IsUsed { get; set; }
	public DateTime? UsedAtUtc { get; set; }
	public DateTime? AssignedAtUtc { get; set; }
}