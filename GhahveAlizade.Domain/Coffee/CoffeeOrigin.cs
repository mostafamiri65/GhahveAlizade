using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Coffee;

public class CoffeeOrigin : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public string? CountryCode { get; set; }
	public string? FlagUrl { get; set; }
	public string? ImageUrl { get; set; }
	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }

	public ICollection<CoffeeRegion> Regions { get; set; } = new List<CoffeeRegion>();
}