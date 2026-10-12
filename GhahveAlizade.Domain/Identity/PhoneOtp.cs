using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Identity;

public class PhoneOtp : BaseEntity
{
	public string PhoneNumber { get; set; } = default!;
	public string Code { get; set; } = default!;      // 6 رقمی
	public string? Purpose { get; set; }              // Login, Register, ResetPassword, VerifyPhone

	public DateTime ExpiresAtUtc { get; set; }
	public int AttemptCount { get; set; }
	public int MaxAttempts { get; set; } = 5;
	public bool IsUsed { get; set; }
	public DateTime? UsedAtUtc { get; set; }

	public string? IpAddress { get; set; }
	public string? UserAgent { get; set; }

	public bool IsValid()
		=> !IsUsed
		   && DateTime.UtcNow < ExpiresAtUtc
		   && AttemptCount < MaxAttempts;
}