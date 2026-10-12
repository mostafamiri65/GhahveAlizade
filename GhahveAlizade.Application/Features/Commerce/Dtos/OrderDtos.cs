using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Commerce.Dtos;

public class OrderListItemDto
{
	public long Id { get; set; }
	public string OrderNumber { get; set; } = string.Empty;

	public long? CustomerId { get; set; }
	public string? CustomerName { get; set; }
	public string? CustomerPhone { get; set; }
	public string? CustomerEmail { get; set; }

	public OrderStatus Status { get; set; }
	public string? StatusName { get; set; }

	public decimal SubTotal { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal ShippingAmount { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal Total { get; set; }

	public string Currency { get; set; } = "IRT";
	public int ItemCount { get; set; }

	public DateTime? PaidAtUtc { get; set; }
	public DateTime? ShippedAtUtc { get; set; }
	public DateTime? DeliveredAtUtc { get; set; }

	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }

	public string? PaymentMethodName { get; set; }
	public string? ShipmentStatusName { get; set; }
}

public class OrderDetailDto : OrderListItemDto
{
	public string? CustomerNotes { get; set; }
	public string? AdminNotes { get; set; }
	public string? CouponCode { get; set; }

	public List<OrderItemDto> Items { get; set; } = new();
	public List<OrderAddressDto> Addresses { get; set; } = new();

	public List<PaymentDto> Payments { get; set; } = new();
	public List<ShipmentDto> Shipments { get; set; } = new();
}

public class OrderItemDto
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public long? ProductVariantId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ProductSku { get; set; }
	public string? ProductImageUrl { get; set; }
	public string? VariantName { get; set; }
	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal TotalPrice { get; set; }
	public string? Notes { get; set; }
}

public class OrderAddressDto
{
	public long Id { get; set; }
	public string AddressType { get; set; } = string.Empty; // Billing, Shipping
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string Province { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string AddressLine1 { get; set; } = string.Empty;
	public string? AddressLine2 { get; set; }
}

public class PaymentDto
{
	public long Id { get; set; }
	public string PaymentNumber { get; set; } = string.Empty;
	public PaymentMethod Method { get; set; }
	public string? MethodName { get; set; }
	public PaymentStatus Status { get; set; }
	public string? StatusName { get; set; }
	public decimal Amount { get; set; }
	public string? GatewayName { get; set; }
	public string? GatewayTransactionId { get; set; }
	public DateTime? PaidAtUtc { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class ShipmentDto
{
	public long Id { get; set; }
	public string ShipmentNumber { get; set; } = string.Empty;
	public ShipmentStatus Status { get; set; }
	public string? StatusName { get; set; }
	public string? CarrierName { get; set; }
	public string? TrackingNumber { get; set; }
	public string? TrackingUrl { get; set; }
	public DateTime? ShippedAtUtc { get; set; }
	public DateTime? DeliveredAtUtc { get; set; }
	public string? Notes { get; set; }
}