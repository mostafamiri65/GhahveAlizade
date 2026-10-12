using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Pricing;

public class PriceList : BaseEntity
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
	public string Currency { get; set; } = "IRT";

	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }

	public bool IsActive { get; set; } = true;
	public bool IsDefault { get; set; }

	public ICollection<PriceListItem> Items { get; set; } = new List<PriceListItem>();
}