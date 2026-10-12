namespace GhahveAlizade.Application.Features.Inventory.Dtos;

public class WarehouseDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public string? Address { get; set; }
	public string? City { get; set; }
	public string? Province { get; set; }
	public string? PostalCode { get; set; }
	public string? Phone { get; set; }
	public string? ManagerName { get; set; }
	public bool IsActive { get; set; }
	public bool IsDefault { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
	public int InventoryItemCount { get; set; }
}

public class WarehouseListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public string? City { get; set; }
	public string? Province { get; set; }
	public string? ManagerName { get; set; }
	public bool IsActive { get; set; }
	public bool IsDefault { get; set; }
	public int InventoryItemCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}