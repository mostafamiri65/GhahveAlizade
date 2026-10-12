using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using Microsoft.AspNetCore.Http;

namespace GhahveAlizade.Web.Areas.Admin.Models.Products;

public class ProductFormViewModel
{
	public long Id { get; set; }

	// ============ اطلاعات اصلی ============
	[Required(ErrorMessage = "نام محصول الزامی است.")]
	[StringLength(500)]
	[Display(Name = "نام محصول")]
	public string Name { get; set; } = string.Empty;

	[StringLength(500)]
	[Display(Name = "نامک (Slug)")]
	public string? Slug { get; set; }

	[StringLength(1000)]
	[Display(Name = "توضیح کوتاه")]
	[DataType(DataType.MultilineText)]
	public string? ShortDescription { get; set; }

	[Display(Name = "توضیحات کامل")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(100)]
	[Display(Name = "SKU")]
	public string? Sku { get; set; }

	[StringLength(100)]
	[Display(Name = "بارکد")]
	public string? Barcode { get; set; }

	// ============ قیمت ============
	[Display(Name = "دارای واریانت")]
	public bool HasVariants { get; set; }

	[Display(Name = "قیمت پایه")]
	[Range(0, double.MaxValue)]
	public decimal? BasePrice { get; set; }

	[Display(Name = "قیمت قبل از تخفیف")]
	[Range(0, double.MaxValue)]
	public decimal? CompareAtPrice { get; set; }

	[Display(Name = "قیمت خرید")]
	[Range(0, double.MaxValue)]
	public decimal? CostPrice { get; set; }

	[Display(Name = "مشمول مالیات")]
	public bool IsTaxable { get; set; } = true;

	[Display(Name = "نرخ مالیات (٪)")]
	[Range(0, 100)]
	public decimal? TaxRate { get; set; }

	// ============ ابعاد ============
	[Display(Name = "وزن (گرم)")]
	[Range(0, double.MaxValue)]
	public decimal? Weight { get; set; }

	[Display(Name = "طول (cm)")]
	[Range(0, double.MaxValue)]
	public decimal? Length { get; set; }

	[Display(Name = "عرض (cm)")]
	[Range(0, double.MaxValue)]
	public decimal? Width { get; set; }

	[Display(Name = "ارتفاع (cm)")]
	[Range(0, double.MaxValue)]
	public decimal? Height { get; set; }

	// ============ روابط ============
	[Display(Name = "دسته‌بندی")]
	public long? CategoryId { get; set; }

	[Display(Name = "برند")]
	public long? BrandId { get; set; }

	[Display(Name = "خاستگاه قهوه")]
	public long? CoffeeOriginId { get; set; }

	[Display(Name = "فرآیند قهوه")]
	public long? CoffeeProcessId { get; set; }

	[Display(Name = "پروفایل رست")]
	public long? RoastProfileId { get; set; }

	[Display(Name = "نُت‌های طعمی")]
	public List<long> FlavorNoteIds { get; set; } = new();

	// ============ وضعیت ============
	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "منتشر شده")]
	public bool IsPublished { get; set; } = true;

	[Display(Name = "ویژه")]
	public bool IsFeatured { get; set; }

	[Display(Name = "جدید")]
	public bool IsNew { get; set; }

	[Display(Name = "پرفروش")]
	public bool IsBestSeller { get; set; }

	// ============ SEO ============
	[StringLength(200)]
	[Display(Name = "عنوان سئو")]
	public string? MetaTitle { get; set; }

	[StringLength(500)]
	[Display(Name = "توضیحات سئو")]
	[DataType(DataType.MultilineText)]
	public string? MetaDescription { get; set; }

	[StringLength(500)]
	[Display(Name = "کلمات کلیدی")]
	public string? MetaKeywords { get; set; }

	[StringLength(1000)]
	[Display(Name = "Canonical URL")]
	public string? CanonicalUrl { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر Open Graph")]
	public string? OgImage { get; set; }

	// ============ Variants ============
	public List<ProductVariantViewModel> Variants { get; set; } = new();

	// ============ Images ============
	public List<ProductImageViewModel> Images { get; set; } = new();

	// ============ Attributes ============
	public List<ProductAttributeViewModel> Attributes { get; set; } = new();

	// ============ آپلود فایل ============
	[Display(Name = "آپلود تصاویر جدید")]
	public List<IFormFile>? NewImages { get; set; }

	// ============ داده‌های کمکی ============
	public ProductFormDataDto FormData { get; set; } = new();
}

public class ProductVariantViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام واریانت الزامی است.")]
	public string Name { get; set; } = string.Empty;

	public string? Sku { get; set; }
	public string? Barcode { get; set; }
	public string? AttributesJson { get; set; }

	[Range(0, double.MaxValue)]
	public decimal Price { get; set; }

	public decimal? CompareAtPrice { get; set; }
	public decimal? CostPrice { get; set; }
	public decimal? Weight { get; set; }
	public string? ImageUrl { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; } = true;
	public bool IsDefault { get; set; }
}

public class ProductImageViewModel
{
	public long Id { get; set; }
	public string ImageUrl { get; set; } = string.Empty;
	public string? ThumbnailUrl { get; set; }
	public string? AltText { get; set; }
	public string? Title { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsPrimary { get; set; }
	public long? FileSizeBytes { get; set; }
	public int? Width { get; set; }
	public int? Height { get; set; }
}

public class ProductAttributeViewModel
{
	public long Id { get; set; }
	public long AttributeId { get; set; }
	public long? AttributeValueId { get; set; }
	public string? CustomValue { get; set; }
	public int DisplayOrder { get; set; }

	// برای نمایش
	public string? AttributeName { get; set; }
	public string? Value { get; set; }
}