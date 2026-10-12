using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Wholesale;

public class WholesaleCustomer : BaseEntity
{
	public long CustomerId { get; set; }
	public Customer.Customer Customer { get; set; } = default!;

	public string? CompanyName { get; set; }
	public string? BusinessType { get; set; }
	public string? EconomicCode { get; set; }
	public string? RegistrationNumber { get; set; }

	public string? ContactPerson { get; set; }
	public string? ContactPhone { get; set; }
	public string? ContactEmail { get; set; }

	public decimal? CreditLimit { get; set; }
	public int? PaymentTermDays { get; set; }

	public WholesaleRequestStatus Status { get; set; } = WholesaleRequestStatus.Pending;
	public DateTime? ApprovedAtUtc { get; set; }
	public string? ApprovedBy { get; set; }

	public string? Notes { get; set; }
}