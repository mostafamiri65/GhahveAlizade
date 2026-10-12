using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Marketing;

public class AbandonedCartNotification : BaseEntity
{
	public long AbandonedCartId { get; set; }
	public AbandonedCart AbandonedCart { get; set; } = default!;

	public string Channel { get; set; } = default!; // Email, SMS
	public string? Subject { get; set; }
	public string? Content { get; set; }

	public bool IsSent { get; set; }
	public DateTime? SentAtUtc { get; set; }
	public bool IsOpened { get; set; }
	public DateTime? OpenedAtUtc { get; set; }
	public bool IsClicked { get; set; }
	public DateTime? ClickedAtUtc { get; set; }
}