using GhahveAlizade.Domain.Catalog;
using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Inventory;

public class StockReservation : BaseEntity
{
	public long ProductId { get; set; }
	public Product Product { get; set; } = default!;

	public long? ProductVariantId { get; set; }
	public ProductVariant? ProductVariant { get; set; }

	public long WarehouseId { get; set; }
	public Warehouse Warehouse { get; set; } = default!;

	public decimal Quantity { get; set; }
	public string ReferenceType { get; set; } = default!;
	public long ReferenceId { get; set; }

	public DateTime ExpiresAtUtc { get; set; }
	public bool IsReleased { get; set; }
	public DateTime? ReleasedAtUtc { get; set; }
}