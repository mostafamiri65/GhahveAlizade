using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Wholesale.Dtos;

public class WholesaleRequestDto
{
	public long Id { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public string? BusinessType { get; set; }
	public string? EconomicCode { get; set; }
	public string? RegistrationNumber { get; set; }
	public string ContactPerson { get; set; } = string.Empty;
	public string ContactPhone { get; set; } = string.Empty;
	public string? ContactEmail { get; set; }
	public string? City { get; set; }
	public string? Province { get; set; }
	public string? Address { get; set; }
	public string? ProductInterests { get; set; }
	public string? EstimatedMonthlyVolume { get; set; }
	public string? Message { get; set; }
	public WholesaleRequestStatus Status { get; set; }
	public string? StatusName { get; set; }
	public DateTime? ReviewedAtUtc { get; set; }
	public string? ReviewedBy { get; set; }
	public string? ReviewNotes { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class WholesaleRequestListItemDto
{
	public long Id { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public string ContactPerson { get; set; } = string.Empty;
	public string ContactPhone { get; set; } = string.Empty;
	public string? City { get; set; }
	public WholesaleRequestStatus Status { get; set; }
	public string? StatusName { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}