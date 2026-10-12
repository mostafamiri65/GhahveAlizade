using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Shipping;

public class Shipment : BaseEntity
{
	public long OrderId { get; set; }
	public Commerce.Order Order { get; set; } = default!;

	public long? ShippingMethodId { get; set; }
	public ShippingMethod? ShippingMethod { get; set; }

	public string ShipmentNumber { get; set; } = default!;
	public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

	public string? CarrierName { get; set; }
	public string? TrackingNumber { get; set; }
	public string? TrackingUrl { get; set; }

	public DateTime? ShippedAtUtc { get; set; }
	public DateTime? DeliveredAtUtc { get; set; }
	public DateTime? EstimatedDeliveryAtUtc { get; set; }

	public string? Notes { get; set; }
}