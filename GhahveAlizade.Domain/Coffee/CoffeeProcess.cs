using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Coffee;

public class CoffeeProcess : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }
	public string? IconUrl { get; set; }
	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }
}