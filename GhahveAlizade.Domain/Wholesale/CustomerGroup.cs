using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Wholesale;

public class CustomerGroup : BaseEntity
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }

	public decimal? DiscountPercent { get; set; }
	public long? PriceListId { get; set; }
	public Pricing.PriceList? PriceList { get; set; }

	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }
}