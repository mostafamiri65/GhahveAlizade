using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Customer;

public class CustomerAddress : BaseEntity
{
	public long CustomerId { get; set; }
	public Customer Customer { get; set; } = default!;

	public string Title { get; set; } = default!;
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

	public bool IsDefault { get; set; }
	public bool IsBilling { get; set; }
	public bool IsShipping { get; set; }
}