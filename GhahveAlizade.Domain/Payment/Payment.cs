using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Payment;

public class Payment : BaseEntity
{
	public long OrderId { get; set; }
	public Commerce.Order Order { get; set; } = default!;

	public string PaymentNumber { get; set; } = default!;
	public PaymentMethod Method { get; set; }
	public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

	public decimal Amount { get; set; }
	public string Currency { get; set; } = "IRT";

	public string? GatewayName { get; set; }
	public string? GatewayTransactionId { get; set; }
	public string? GatewayReferenceId { get; set; }
	public string? GatewayResponse { get; set; }

	public DateTime? PaidAtUtc { get; set; }
	public DateTime? FailedAtUtc { get; set; }
	public string? FailureReason { get; set; }

	public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
}