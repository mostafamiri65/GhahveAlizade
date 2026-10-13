using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Customer;

public class Customer : BaseEntity
{
	public long? UserId { get; set; }

	public string? FirstName { get; set; }
	public string? LastName { get; set; }
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

	public bool IsActive { get; set; } = true;
	public bool IsVerified { get; set; }
	public bool AcceptsMarketing { get; set; }

	public long? CustomerGroupId { get; set; }
	public Wholesale.CustomerGroup? CustomerGroup { get; set; }

	public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
	public ICollection<CustomerNote> Notes { get; set; } = new List<CustomerNote>();
}