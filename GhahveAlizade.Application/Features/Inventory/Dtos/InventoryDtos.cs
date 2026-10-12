namespace GhahveAlizade.Application.Features.Inventory.Dtos;

public class InventoryItemDto
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ProductSlug { get; set; }
	public string? ProductSku { get; set; }
	public long? ProductVariantId { get; set; }
	public string? VariantName { get; set; }
	public long WarehouseId { get; set; }
	public string WarehouseName { get; set; } = string.Empty;
	public decimal Quantity { get; set; }
	public decimal ReservedQuantity { get; set; }
	public decimal? ReorderLevel { get; set; }
	public decimal AvailableQuantity => Quantity - ReservedQuantity;
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class InventoryItemListItemDto
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ProductSku { get; set; }
	public long? ProductVariantId { get; set; }
	public string? VariantName { get; set; }
	public long WarehouseId { get; set; }
	public string WarehouseName { get; set; } = string.Empty;
	public decimal Quantity { get; set; }
	public decimal ReservedQuantity { get; set; }
	public decimal? ReorderLevel { get; set; }
	public decimal AvailableQuantity => Quantity - ReservedQuantity;
	public bool IsLowStock => ReorderLevel.HasValue && AvailableQuantity <= ReorderLevel.Value;
	public DateTime UpdatedAtUtc { get; set; }
}

public class InventoryTransactionDto
{
	public long Id { get; set; }
	public string TransactionType { get; set; } = string.Empty;
	public decimal Quantity { get; set; }
	public decimal BalanceAfter { get; set; }
	public string? ReferenceType { get; set; }
	public long? ReferenceId { get; set; }
	public string? Notes { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public string? CreatedBy { get; set; }
}