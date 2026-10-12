using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Pricing;

public class QuantityPrice : BaseEntity
{
	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	public int MinQuantity { get; set; }
	public int? MaxQuantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal? DiscountPercent { get; set; }
}