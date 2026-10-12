using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Accounting;

public class AccountingMapping : BaseEntity
{
	public string LocalEntityType { get; set; } = default!;
	public long LocalEntityId { get; set; }

	public string ExternalEntityType { get; set; } = default!;
	public string ExternalEntityId { get; set; } = default!;
	public string? ExternalSystem { get; set; }

	public DateTime? LastSyncedAtUtc { get; set; }
}