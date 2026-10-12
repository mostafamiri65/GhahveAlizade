using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Promotion;

public class Coupon : BaseEntity
{
	public string Code { get; set; } = default!;
	public string? Description { get; set; }

	public DiscountType DiscountType { get; set; }
	public decimal DiscountValue { get; set; }
	public decimal? MaxDiscountAmount { get; set; }
	public decimal? MinOrderAmount { get; set; }

	public int? MaxUsageCount { get; set; }
	public int UsedCount { get; set; }
	public int? MaxUsagePerCustomer { get; set; }

	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }

	public bool IsActive { get; set; } = true;
	public bool IsFirstOrderOnly { get; set; }
	public bool IsPublic { get; set; } = true;

	// محدودیت‌ها
	public string? ApplicableProductIdsJson { get; set; }
	public string? ApplicableCategoryIdsJson { get; set; }
	public string? ApplicableBrandIdsJson { get; set; }
	public string? ExcludedProductIdsJson { get; set; }

	public ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
}