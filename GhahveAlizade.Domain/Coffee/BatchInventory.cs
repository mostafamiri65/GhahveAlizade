using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Inventory;

namespace GhahveAlizade.Domain.Coffee;

public class BatchInventory : BaseEntity
{
	public long ProductBatchId { get; set; }
	public ProductBatch ProductBatch { get; set; } = default!;

	public long WarehouseId { get; set; }
	public Warehouse Warehouse { get; set; } = default!;

	public decimal Quantity { get; set; }
	public decimal ReservedQuantity { get; set; }
}