namespace GhahveAlizade.Application.Features.Catalog.Products.Dtos;

public class ProductListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Sku { get; set; }

	// قیمت
	public decimal? BasePrice { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? MinVariantPrice { get; set; }
	public decimal? MaxVariantPrice { get; set; }
	public bool HasVariants { get; set; }

	// تصویر اصلی
	public string? PrimaryImageUrl { get; set; }

	// روابط
	public long? CategoryId { get; set; }
	public string? CategoryName { get; set; }

	public long? BrandId { get; set; }
	public string? BrandName { get; set; }

	// ویژگی‌های قهوه
	public string? CoffeeOriginName { get; set; }
	public string? RoastProfileName { get; set; }
	public string? CoffeeProcessName { get; set; }

	// وضعیت
	public bool IsActive { get; set; }
	public bool IsPublished { get; set; }
	public bool IsFeatured { get; set; }
	public bool IsNew { get; set; }
	public bool IsBestSeller { get; set; }

	// آمار
	public int ViewCount { get; set; }
	public int SoldCount { get; set; }
	public decimal AverageRating { get; set; }
	public int ReviewCount { get; set; }

	// موجودی
	public int TotalStock { get; set; }

	public DateTime CreatedAtUtc { get; set; }
}