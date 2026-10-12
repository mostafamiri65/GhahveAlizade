using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Coffee;

public class CoffeeRegion : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }

	public long CoffeeOriginId { get; set; }
	public CoffeeOrigin CoffeeOrigin { get; set; } = default!;

	public string? Altitude { get; set; }
	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }
}