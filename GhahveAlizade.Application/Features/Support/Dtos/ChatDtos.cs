using GhahveAlizade.Domain.Support;

namespace GhahveAlizade.Application.Features.Support.Dtos;

public class ChatSessionDto
{
	public long Id { get; set; }
	public ChatSessionStatus Status { get; set; }
	public string? StatusName { get; set; }
	public string? Subject { get; set; }
	public string? CustomerName { get; set; }
	public string? GuestName { get; set; }
	public string? GuestPhoneNumber { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public int UnreadCount { get; set; }
}

public class ChatMessageDto
{
	public long Id { get; set; }
	public string? SenderName { get; set; }
	public bool IsFromAdmin { get; set; }
	public string Body { get; set; } = string.Empty;
	public DateTime CreatedAtUtc { get; set; }
}

public class ChatHistoryDto
{
	public long SessionId { get; set; }
	public ChatSessionStatus Status { get; set; }
	public string? CustomerName { get; set; }
	public string? GuestName { get; set; }
	public List<ChatMessageDto> Messages { get; set; } = new();
}
