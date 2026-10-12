using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Commerce;

public class OrderAddress : BaseEntity
{
	public long OrderId { get; set; }
	public Order Order { get; set; } = default!;

	public string AddressType { get; set; } = default!; // Billing, Shipping

	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string PhoneNumber { get; set; } = default!;
	public string? Email { get; set; }

	public string Province { get; set; } = default!;
	public string City { get; set; } = default!;
	public string PostalCode { get; set; } = default!;
	public string AddressLine1 { get; set; } = default!;
	public string? AddressLine2 { get; set; }

	public decimal? Latitude { get; set; }
	public decimal? Longitude { get; set; }
}