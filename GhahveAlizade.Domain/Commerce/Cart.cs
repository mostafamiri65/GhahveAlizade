using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Commerce;

public class Cart : BaseEntity
{
	public string? UserId { get; set; }
	public string? SessionId { get; set; }
	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public string? CouponCode { get; set; }
	public decimal SubTotal { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal ShippingAmount { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal Total { get; set; }

	public string Currency { get; set; } = "IRT";
	public bool IsActive { get; set; } = true;
	public DateTime? ExpiresAtUtc { get; set; }

	public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}