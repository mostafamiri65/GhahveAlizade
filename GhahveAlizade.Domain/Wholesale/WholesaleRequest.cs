using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Wholesale;

public class WholesaleRequest : BaseEntity
{
	public string CompanyName { get; set; } = default!;
	public string? BusinessType { get; set; }
	public string? EconomicCode { get; set; }
	public string? RegistrationNumber { get; set; }

	public string ContactPerson { get; set; } = default!;
	public string ContactPhone { get; set; } = default!;
	public string? ContactEmail { get; set; }

	public string? City { get; set; }
	public string? Province { get; set; }
	public string? Address { get; set; }

	public string? ProductInterests { get; set; }
	public string? EstimatedMonthlyVolume { get; set; }
	public string? Message { get; set; }

	public WholesaleRequestStatus Status { get; set; } = WholesaleRequestStatus.Pending;
	public DateTime? ReviewedAtUtc { get; set; }
	public string? ReviewedBy { get; set; }
	public string? ReviewNotes { get; set; }
}