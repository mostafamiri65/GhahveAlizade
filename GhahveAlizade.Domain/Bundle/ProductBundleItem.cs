using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Bundle;

public class ProductBundleItem : BaseEntity
{
	public long ProductBundleId { get; set; }
	public ProductBundle ProductBundle { get; set; } = default!;

	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	public int Quantity { get; set; } = 1;
	public int DisplayOrder { get; set; }
}