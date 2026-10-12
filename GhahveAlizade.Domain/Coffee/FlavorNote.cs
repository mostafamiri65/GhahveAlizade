using GhahveAlizade.Domain.Catalog;
using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Coffee;

public class FlavorNote : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Category { get; set; }
	public string? IconUrl { get; set; }
	public string? ColorCode { get; set; }
	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }

	public ICollection<Product> Products { get; set; } = new List<Product>();
}