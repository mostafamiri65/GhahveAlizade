using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Promotion.Dtos;

public class CouponDto
{
	public long Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public string? Description { get; set; }
	public DiscountType DiscountType { get; set; }
	public string? DiscountTypeName { get; set; }
	public decimal DiscountValue { get; set; }
	public decimal? MaxDiscountAmount { get; set; }
	public decimal? MinOrderAmount { get; set; }
	public int? MaxUsageCount { get; set; }
	public int UsedCount { get; set; }
	public int? MaxUsagePerCustomer { get; set; }
	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }
	public bool IsActive { get; set; }
	public bool IsFirstOrderOnly { get; set; }
	public bool IsPublic { get; set; }
	public string? ApplicableProductIdsJson { get; set; }
	public string? ApplicableCategoryIdsJson { get; set; }
	public string? ApplicableBrandIdsJson { get; set; }
	public string? ExcludedProductIdsJson { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class CouponListItemDto
{
	public long Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public DiscountType DiscountType { get; set; }
	public string? DiscountTypeName { get; set; }
	public decimal DiscountValue { get; set; }
	public int? MaxUsageCount { get; set; }
	public int UsedCount { get; set; }
	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }
	public bool IsActive { get; set; }
	public bool IsPublic { get; set; }
	public string? Status { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}