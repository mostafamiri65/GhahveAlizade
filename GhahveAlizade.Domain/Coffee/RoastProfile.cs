using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Coffee;

public class RoastProfile : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public string? ColorCode { get; set; }
	public int RoastLevel { get; set; }
	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }
}