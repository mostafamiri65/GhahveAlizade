using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Coffee;

public class ProductBatch : BaseEntity
{
	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public string BatchNumber { get; set; } = default!;
	public DateOnly RoastDate { get; set; }
	public DateOnly? ExpiryDate { get; set; }
	public string? RoastLevel { get; set; }
	public string? RoasterNotes { get; set; }

	public decimal TotalWeight { get; set; }
	public decimal RemainingWeight { get; set; }

	public bool IsActive { get; set; } = true;
}