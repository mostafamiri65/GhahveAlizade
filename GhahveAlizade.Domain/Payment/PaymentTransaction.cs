using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Payment;

public class PaymentTransaction : BaseEntity
{
	public long PaymentId { get; set; }
	public Payment Payment { get; set; } = default!;

	public string TransactionType { get; set; } = default!; // Verify, Refund, Cancel
	public decimal Amount { get; set; }
	public bool IsSuccessful { get; set; }

	public string? RequestPayload { get; set; }
	public string? ResponsePayload { get; set; }
	public string? ErrorMessage { get; set; }

	public DateTime OccurredAtUtc { get; set; }
}