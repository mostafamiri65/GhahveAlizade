using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Bundles.Queries.GetBundleFormData;

namespace GhahveAlizade.Web.Areas.Admin.Models.Bundles;

public class BundleFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام بسته الزامی است.")]
	[StringLength(500)]
	[Display(Name = "نام بسته")]
	public string Name { get; set; } = string.Empty;

	[StringLength(500)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(1000)]
	[Display(Name = "توضیح کوتاه")]
	public string? ShortDescription { get; set; }

	[Display(Name = "توضیحات کامل")]
	public string? Description { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر")]
	public string? ImageUrl { get; set; }

	[Required]
	[Range(0.01, double.MaxValue, ErrorMessage = "قیمت بسته الزامی است.")]
	[Display(Name = "قیمت بسته")]
	public decimal BundlePrice { get; set; }

	[Range(0, double.MaxValue)]
	[Display(Name = "قیمت قبل از تخفیف")]
	public decimal? CompareAtPrice { get; set; }

	[Range(0, double.MaxValue)]
	[Display(Name = "جمع قیمت اقلام")]
	public decimal? TotalItemsPrice { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "ویژه")]
	public bool IsFeatured { get; set; }

	[Display(Name = "تاریخ شروع")]
	public DateTime? StartsAtUtc { get; set; }

	[Display(Name = "تاریخ پایان")]
	public DateTime? ExpiresAtUtc { get; set; }

	[StringLength(200)]
	[Display(Name = "عنوان سئو")]
	public string? MetaTitle { get; set; }

	[StringLength(500)]
	[Display(Name = "توضیحات سئو")]
	public string? MetaDescription { get; set; }

	[StringLength(1000)]
	[Display(Name = "Canonical URL")]
	public string? CanonicalUrl { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر OG")]
	public string? OgImage { get; set; }

	public List<BundleItemViewModel> Items { get; set; } = new();
	public BundleFormDataDto FormData { get; set; } = new();
}

public class BundleItemViewModel
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public string? ProductName { get; set; }
	public long? ProductVariantId { get; set; }
	public string? VariantName { get; set; }
	public int Quantity { get; set; } = 1;
	public int DisplayOrder { get; set; }
}