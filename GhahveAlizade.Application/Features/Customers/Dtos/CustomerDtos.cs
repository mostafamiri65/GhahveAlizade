namespace GhahveAlizade.Application.Features.Customers.Dtos;

public class CustomerDto
{
	public long Id { get; set; }
	public string? UserId { get; set; }

	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? FullName { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? NationalCode { get; set; }
	public DateOnly? BirthDate { get; set; }
	public string? Gender { get; set; }
	public string? AvatarUrl { get; set; }

	public int TotalOrders { get; set; }
	public decimal TotalSpent { get; set; }
	public DateTime? LastOrderAtUtc { get; set; }
	public DateTime? LastLoginAtUtc { get; set; }

	public bool IsActive { get; set; }
	public bool IsVerified { get; set; }
	public bool AcceptsMarketing { get; set; }

	public long? CustomerGroupId { get; set; }
	public string? CustomerGroupName { get; set; }

	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }

	public List<CustomerAddressDto> Addresses { get; set; } = new();
	public List<CustomerNoteDto> Notes { get; set; } = new();
}

public class CustomerListItemDto
{
	public long Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? FullName { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? CustomerGroupName { get; set; }
	public int TotalOrders { get; set; }
	public decimal TotalSpent { get; set; }
	public DateTime? LastOrderAtUtc { get; set; }
	public bool IsActive { get; set; }
	public bool IsVerified { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class CustomerAddressDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string Province { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string AddressLine1 { get; set; } = string.Empty;
	public string? AddressLine2 { get; set; }
	public bool IsDefault { get; set; }
	public bool IsBilling { get; set; }
	public bool IsShipping { get; set; }
}

public class CustomerNoteDto
{
	public long Id { get; set; }
	public string Content { get; set; } = string.Empty;
	public string? Category { get; set; }
	public bool IsInternal { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public string? CreatedBy { get; set; }
}

public class CustomerGroupDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public decimal? DiscountPercent { get; set; }
	public long? PriceListId { get; set; }
	public string? PriceListName { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public int CustomerCount { get; set; }
}