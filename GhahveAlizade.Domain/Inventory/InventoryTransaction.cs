using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Inventory;

public class InventoryTransaction : BaseEntity
{
	public long InventoryItemId { get; set; }
	public InventoryItem InventoryItem { get; set; } = default!;

	public string TransactionType { get; set; } = default!;
	public decimal Quantity { get; set; }
	public decimal BalanceAfter { get; set; }

	public string? ReferenceType { get; set; }
	public long? ReferenceId { get; set; }
	public string? Notes { get; set; }
}