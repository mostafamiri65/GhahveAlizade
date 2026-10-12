using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Shipping;

public class ShippingMethod : BaseEntity
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
	public string? Code { get; set; }

	public decimal Price { get; set; }
	public decimal? FreeShippingThreshold { get; set; }

	public int? EstimatedDeliveryDaysMin { get; set; }
	public int? EstimatedDeliveryDaysMax { get; set; }

	public string? ImageUrl { get; set; }
	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }

	public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}