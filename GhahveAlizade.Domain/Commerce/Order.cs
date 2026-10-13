using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Payment;
using GhahveAlizade.Domain.Shipping;
using System.ComponentModel.DataAnnotations.Schema;

namespace GhahveAlizade.Domain.Commerce;

public class Order : BaseEntity
{
	public string OrderNumber { get; set; } = default!;

	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }
	public long? UserId { get; set; }

	public OrderStatus Status { get; set; } = OrderStatus.Pending;

	public string? CouponCode { get; set; }
	public decimal SubTotal { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal ShippingAmount { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal Total { get; set; }

	public string Currency { get; set; } = "IRT";

	public string? CustomerNotes { get; set; }
	public string? AdminNotes { get; set; }

	public DateTime? PaidAtUtc { get; set; }
	public DateTime? ShippedAtUtc { get; set; }
	public DateTime? DeliveredAtUtc { get; set; }
	public DateTime? CancelledAtUtc { get; set; }

	public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
	public ICollection<OrderAddress> Addresses { get; set; } = new List<OrderAddress>();

	public ICollection<Payment.Payment> Payments { get; set; } = new List<Payment.Payment>();

	public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}