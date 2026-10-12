using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Promotion;

public class CouponUsage : BaseEntity
{
	public long CouponId { get; set; }
	public Coupon Coupon { get; set; } = default!;

	public long OrderId { get; set; }
	public Commerce.Order Order { get; set; } = default!;

	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public decimal DiscountAmount { get; set; }
	public DateTime UsedAtUtc { get; set; }
}