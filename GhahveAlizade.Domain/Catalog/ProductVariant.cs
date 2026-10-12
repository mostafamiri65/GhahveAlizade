using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Catalog;

public class ProductVariant : BaseEntity
{
	public long ProductId { get; set; }
	public Product Product { get; set; } = default!;

	public string Name { get; set; } = default!;
	public string? Sku { get; set; }
	public string? Barcode { get; set; }
	public string? AttributesJson { get; set; }

	public decimal Price { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? CostPrice { get; set; }

	public decimal? Weight { get; set; }

	public string? ImageUrl { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; } = true;
	public bool IsDefault { get; set; }
}