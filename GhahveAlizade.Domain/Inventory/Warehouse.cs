using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Inventory;

public class Warehouse : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Code { get; set; } = default!;
	public string? Address { get; set; }
	public string? City { get; set; }
	public string? Province { get; set; }
	public string? PostalCode { get; set; }
	public string? Phone { get; set; }
	public string? ManagerName { get; set; }
	public bool IsActive { get; set; } = true;
	public bool IsDefault { get; set; }
}