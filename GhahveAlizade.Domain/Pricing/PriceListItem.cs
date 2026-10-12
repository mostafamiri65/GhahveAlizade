using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Pricing;

public class PriceListItem : BaseEntity
{
	public long PriceListId { get; set; }
	public PriceList PriceList { get; set; } = default!;

	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	public decimal Price { get; set; }
	public decimal? CompareAtPrice { get; set; }
	public decimal? MinQuantity { get; set; }
}