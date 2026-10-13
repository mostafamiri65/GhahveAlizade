using GhahveAlizade.Domain.Support;

namespace GhahveAlizade.Application.Features.Support.Dtos;

public class TicketListItemDto
{
	public long Id { get; set; }
	public string TicketNumber { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public TicketStatus Status { get; set; }
	public string? StatusName { get; set; }
	public TicketPriority Priority { get; set; }
	public int MessageCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? LastMessageAtUtc { get; set; }
	public bool HasUnreadAdminReply { get; set; }
}
public class TicketDetailWithMetaDto
{
	public long Id { get; set; }
	public string TicketNumber { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public TicketStatus Status { get; set; }
	public long? RelatedOrderId { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public string? CustomerName { get; set; }
	public string? CustomerPhone { get; set; }
	public long? AssignedToUserId { get; set; }
	public string? AssignedToName { get; set; }
	public List<TicketMessageDto> Messages { get; set; } = new();
}
public class TicketDetailDto
{
	public long Id { get; set; }
	public string TicketNumber { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public TicketStatus Status { get; set; }
	public string? StatusName { get; set; }
	public long? RelatedOrderId { get; set; }
	public DateTime CreatedAtUtc { get; set; }

	public List<TicketMessageDto> Messages { get; set; } = new();
}

public class TicketDetailWithMetaDto
{
	public long Id { get; set; }
	public string TicketNumber { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public TicketStatus Status { get; set; }
	
	public long? RelatedOrderId { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public string? CustomerName { get; set; }
	public string? CustomerPhone { get; set; }
	public long? AssignedToUserId { get; set; }
	public string? AssignedToName { get; set; }
	public List<TicketMessageDto> Messages { get; set; } = new();
}
public class TicketMessageDto
{
	public long Id { get; set; }
	public string? SenderName { get; set; }
	public bool IsFromAdmin { get; set; }
	public string Body { get; set; } = string.Empty;
	public DateTime CreatedAtUtc { get; set; }
}

// ---- Admin side ----
public class AdminTicketListItemDto : TicketListItemDto
{
	public string? CustomerName { get; set; }
	public string? CustomerPhone { get; set; }
	public long? AssignedToUserId { get; set; }
	public string? AssignedToName { get; set; }
	public bool HasUnreadCustomerMessage { get; set; }
}
