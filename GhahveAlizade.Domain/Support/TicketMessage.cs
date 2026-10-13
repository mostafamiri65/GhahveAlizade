using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Identity;

namespace GhahveAlizade.Domain.Support;

public class TicketMessage : BaseEntity
{
	public long TicketId { get; set; }
	public SupportTicket Ticket { get; set; } = default!;

	public long? SenderUserId { get; set; }        // ApplicationUser.Id
	public string? SenderName { get; set; }
	public bool IsFromAdmin { get; set; }

	public string Body { get; set; } = default!;
	public string? AttachmentUrl { get; set; }

	public bool IsInternalNote { get; set; }       // یادداشت داخلی ادمین — برای کاربر نمایش داده نمیشود
	public DateTime? ReadAtUtc { get; set; }
}

// ==================== چت آنلاین ====================

public class SupportChatSession : BaseEntity
{
	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public long? UserId { get; set; }
	public string? GuestName { get; set; }
	public string? GuestPhoneNumber { get; set; }

	public long? AssignedToUserId { get; set; }    // ادمین درگیر چت
	public ApplicationUser? AssignedTo { get; set; }

	public ChatSessionStatus Status { get; set; } = ChatSessionStatus.Waiting;
	public string? Subject { get; set; }

	public DateTime? StartedAtUtc { get; set; }
	public DateTime? ClosedAtUtc { get; set; }
	public long? ClosedByUserId { get; set; }

	public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}

public enum ChatSessionStatus
{
	Waiting = 0,     // منتظر اپراتور
	Active = 1,      // در حال گفتگو
	Closed = 2       // بسته شده
}

public class ChatMessage : BaseEntity
{
	public long SessionId { get; set; }
	public SupportChatSession Session { get; set; } = default!;

	public long? SenderUserId { get; set; }
	public string? SenderName { get; set; }
	public bool IsFromAdmin { get; set; }

	public string Body { get; set; } = default!;
	public DateTime? ReadAtUtc { get; set; }
}
