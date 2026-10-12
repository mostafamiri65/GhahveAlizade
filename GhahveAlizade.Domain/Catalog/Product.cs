using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Coffee;
using GhahveAlizade.Domain.Review;

namespace GhahveAlizade.Domain.Catalog;

public class Product : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? ShortDescription { get; set; }
	public string? Description { get; set; }
	public string? Sku { get; set; }
	public string? Barcode { get; set; }

	public bool HasVariants { get; set; }

	public decimal? BasePrice { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? CostPrice { get; set; }

	public bool IsTaxable { get; set; } = true;
	public decimal? TaxRate { get; set; }

	public decimal? Weight { get; set; }
	public decimal? Length { get; set; }
	public decimal? Width { get; set; }
	public decimal? Height { get; set; }

	public long? CategoryId { get; set; }
	public Category? Category { get; set; }

	public long? BrandId { get; set; }
	public Brand? Brand { get; set; }

	public long? CoffeeOriginId { get; set; }
	public CoffeeOrigin? CoffeeOrigin { get; set; }

	public long? CoffeeProcessId { get; set; }
	public CoffeeProcess? CoffeeProcess { get; set; }

	public long? RoastProfileId { get; set; }
	public RoastProfile? RoastProfile { get; set; }

	public bool IsActive { get; set; } = true;
	public bool IsFeatured { get; set; }
	public bool IsNew { get; set; }
	public bool IsBestSeller { get; set; }
	public bool IsPublished { get; set; } = true;
	public DateTime? PublishedAtUtc { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public string? JsonLdSchema { get; set; }

	public int ViewCount { get; set; }
	public int SoldCount { get; set; }
	public decimal AverageRating { get; set; }
	public int ReviewCount { get; set; }

	public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
	public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
	public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
	public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
	public ICollection<ProductBatch> Batches { get; set; } = new List<ProductBatch>();
	public ICollection<FlavorNote> FlavorNotes { get; set; } = new List<FlavorNote>();
	public ICollection<ProductRelation> Relations { get; set; } = new List<ProductRelation>();
}