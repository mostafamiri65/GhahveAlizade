using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Commerce;

public class OrderItem : BaseEntity
{
	public long OrderId { get; set; }
	public Order Order { get; set; } = default!;

	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	// Snapshot اطلاعات محصول در زمان خرید
	public string ProductName { get; set; } = default!;
	public string? ProductSku { get; set; }
	public string? ProductImageUrl { get; set; }
	public string? VariantName { get; set; }

	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal TotalPrice { get; set; }

	public string? Notes { get; set; }
}