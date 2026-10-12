using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.SEO;

public class Redirect : BaseEntity
{
	public string FromPath { get; set; } = default!;
	public string ToPath { get; set; } = default!;
	public RedirectType Type { get; set; } = RedirectType.Permanent301;
	public bool IsActive { get; set; } = true;
	public int HitCount { get; set; }
	public DateTime? LastHitAtUtc { get; set; }
	public string? Notes { get; set; }
}