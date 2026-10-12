namespace GhahveAlizade.Application.Features.Catalog.Products.Dtos;

public class ProductDetailDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? ShortDescription { get; set; }
	public string? Description { get; set; }
	public string? Sku { get; set; }
	public string? Barcode { get; set; }

	public bool HasVariants { get; set; }

	public decimal? BasePrice { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? CostPrice { get; set; }

	public bool IsTaxable { get; set; }
	public decimal? TaxRate { get; set; }

	public decimal? Weight { get; set; }
	public decimal? Length { get; set; }
	public decimal? Width { get; set; }
	public decimal? Height { get; set; }

	// روابط
	public long? CategoryId { get; set; }
	public string? CategoryName { get; set; }

	public long? BrandId { get; set; }
	public string? BrandName { get; set; }

	public long? CoffeeOriginId { get; set; }
	public string? CoffeeOriginName { get; set; }

	public long? CoffeeProcessId { get; set; }
	public string? CoffeeProcessName { get; set; }

	public long? RoastProfileId { get; set; }
	public string? RoastProfileName { get; set; }

	// FlavorNotes
	public List<long> FlavorNoteIds { get; set; } = new();
	public List<string> FlavorNoteNames { get; set; } = new();

	// وضعیت
	public bool IsActive { get; set; }
	public bool IsPublished { get; set; }
	public bool IsFeatured { get; set; }
	public bool IsNew { get; set; }
	public bool IsBestSeller { get; set; }

	public DateTime? PublishedAtUtc { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public string? JsonLdSchema { get; set; }

	// آمار
	public int ViewCount { get; set; }
	public int SoldCount { get; set; }
	public decimal AverageRating { get; set; }
	public int ReviewCount { get; set; }

	// مجموعه‌ها
	public List<ProductVariantDto> Variants { get; set; } = new();
	public List<ProductImageDto> Images { get; set; } = new();
	public List<ProductAttributeDto> Attributes { get; set; } = new();

	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}