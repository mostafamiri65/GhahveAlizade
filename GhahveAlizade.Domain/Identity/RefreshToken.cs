using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Identity;

public class RefreshToken : BaseEntity
{
	public long UserId { get; set; }
	public string Token { get; set; } = default!;
	public DateTime ExpiresAtUtc { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime? RevokedAtUtc { get; set; }
	public string? ReplacedByToken { get; set; }
}