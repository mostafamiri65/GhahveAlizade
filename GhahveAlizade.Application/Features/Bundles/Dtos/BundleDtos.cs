namespace GhahveAlizade.Application.Features.Bundles.Dtos;

public class ProductBundleDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? ShortDescription { get; set; }
	public string? ImageUrl { get; set; }
	public decimal BundlePrice { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? TotalItemsPrice { get; set; }
	public bool IsActive { get; set; }
	public bool IsFeatured { get; set; }
	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public List<ProductBundleItemDto> Items { get; set; } = new();
}

public class ProductBundleItemDto
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ProductImageUrl { get; set; }
	public long? ProductVariantId { get; set; }
	public string? VariantName { get; set; }
	public int Quantity { get; set; }
	public int DisplayOrder { get; set; }
}

public class ProductBundleListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? ImageUrl { get; set; }
	public decimal BundlePrice { get; set; }
	public decimal? TotalItemsPrice { get; set; }
	public int ItemCount { get; set; }
	public bool IsActive { get; set; }
	public bool IsFeatured { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}