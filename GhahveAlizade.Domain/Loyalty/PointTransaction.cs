using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Loyalty;

public class PointTransaction : BaseEntity
{
	public long CustomerPointAccountId { get; set; }
	public CustomerPointAccount CustomerPointAccount { get; set; } = default!;

	public PointTransactionType Type { get; set; }
	public int Points { get; set; } // مثبت یا منفی
	public int BalanceAfter { get; set; }

	public string? ReferenceType { get; set; }
	public long? ReferenceId { get; set; }
	public string? Description { get; set; }

	public DateTime? ExpiresAtUtc { get; set; }
	public bool IsExpired { get; set; }
}