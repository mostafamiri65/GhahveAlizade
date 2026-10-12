using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Accounting;

public class AccountingSyncLog : BaseEntity
{
	public string EntityType { get; set; } = default!; // Order, Payment, Refund
	public long EntityId { get; set; }

	public string Direction { get; set; } = default!; // Push, Pull
	public string? ExternalSystem { get; set; }

	public bool IsSuccessful { get; set; }
	public string? RequestPayload { get; set; }
	public string? ResponsePayload { get; set; }
	public string? ErrorMessage { get; set; }

	public DateTime SyncedAtUtc { get; set; }
}