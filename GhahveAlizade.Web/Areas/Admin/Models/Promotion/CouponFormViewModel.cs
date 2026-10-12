using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Web.Areas.Admin.Models.Promotion;

public class CouponFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "کد تخفیف الزامی است.")]
	[StringLength(50, MinimumLength = 3)]
	[Display(Name = "کد تخفیف")]
	public string Code { get; set; } = string.Empty;

	[StringLength(1000)]
	[Display(Name = "توضیحات")]
	public string? Description { get; set; }

	[Required]
	[Display(Name = "نوع تخفیف")]
	public DiscountType DiscountType { get; set; }

	[Required(ErrorMessage = "مقدار تخفیف الزامی است.")]
	[Display(Name = "مقدار تخفیف")]
	public decimal DiscountValue { get; set; }

	[Display(Name = "حداکثر تخفیف")]
	public decimal? MaxDiscountAmount { get; set; }

	[Display(Name = "حداقل مبلغ سفارش")]
	public decimal? MinOrderAmount { get; set; }

	[Display(Name = "حداکثر تعداد استفاده")]
	public int? MaxUsageCount { get; set; }

	[Display(Name = "حداکثر استفاده هر کاربر")]
	public int? MaxUsagePerCustomer { get; set; }

	[Display(Name = "تاریخ شروع")]
	public DateTime? StartsAtUtc { get; set; }

	[Display(Name = "تاریخ پایان")]
	public DateTime? ExpiresAtUtc { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "فقط اولین سفارش")]
	public bool IsFirstOrderOnly { get; set; }

	[Display(Name = "عمومی (نمایش در سایت)")]
	public bool IsPublic { get; set; } = true;
}