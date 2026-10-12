using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Bundle;

public class ProductBundle : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public string? ShortDescription { get; set; }

	public string? ImageUrl { get; set; }

	public decimal BundlePrice { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? TotalItemsPrice { get; set; }

	public bool IsActive { get; set; } = true;
	public bool IsFeatured { get; set; }
	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }

	public ICollection<ProductBundleItem> Items { get; set; } = new List<ProductBundleItem>();
}