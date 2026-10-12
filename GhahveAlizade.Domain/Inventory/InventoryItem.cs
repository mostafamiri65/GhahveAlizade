using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Inventory;

public class InventoryItem : BaseEntity
{
	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public Catalog.ProductVariant? ProductVariant { get; set; }

	public long WarehouseId { get; set; }
	public Warehouse Warehouse { get; set; } = default!;

	public decimal Quantity { get; set; }
	public decimal ReservedQuantity { get; set; }
	public decimal? ReorderLevel { get; set; }
}