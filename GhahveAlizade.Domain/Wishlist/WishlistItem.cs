using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Wishlist;

public class WishlistItem : BaseEntity
{
	public long WishlistId { get; set; }
	public Wishlist Wishlist { get; set; } = default!;

	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	public string? Notes { get; set; }
}