using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Support;

public class SupportTicket : BaseEntity
{
	public string TicketNumber { get; set; } = default!;   // مثلا TK-20260304-0001

	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public long? UserId { get; set; }                      // ApplicationUser.Id (برای کاربران بدون رکورد Customer)
	public string? GuestName { get; set; }                 // برای کاربران مهمان (اختیاری)
	public string? GuestPhoneNumber { get; set; }

	public string Subject { get; set; } = default!;
	public string Body { get; set; } = default!;
	public string Category { get; set; } = "General";      // General, Order, Shipping, Payment, Product, Complaint

	public TicketStatus Status { get; set; } = TicketStatus.Open;
	public TicketPriority Priority { get; set; } = TicketPriority.Normal;

	public long? AssignedToUserId { get; set; }            // ادمین مسئول
	public ApplicationUser? AssignedTo { get; set; }

	public DateTime? FirstResponseAtUtc { get; set; }
	public DateTime? ResolvedAtUtc { get; set; }
	public DateTime? ClosedAtUtc { get; set; }

	public long? RelatedOrderId { get; set; }

	public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
}

public enum TicketStatus
{
	Open = 0,          // جدید
	InProgress = 1,    // در حال بررسی
	WaitingCustomer = 2, // منتظر پاسخ مشتری
	Resolved = 3,      // حل شده
	Closed = 4         // بسته شده
}

public enum TicketPriority
{
	Low = 0,
	Normal = 1,
	High = 2,
	Urgent = 3
}
