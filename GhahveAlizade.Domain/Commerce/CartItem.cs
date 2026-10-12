using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Commerce;

public class CartItem : BaseEntity
{
	public long CartId { get; set; }
	public Cart Cart { get; set; } = default!;

	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal TotalPrice { get; set; }

	public string? Notes { get; set; }
}