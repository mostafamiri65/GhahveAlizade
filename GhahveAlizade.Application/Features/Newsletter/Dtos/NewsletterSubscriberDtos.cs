namespace GhahveAlizade.Application.Features.Newsletter.Dtos;

public class NewsletterSubscriberListItemDto
{
	public long Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Source { get; set; }
	public string? Tags { get; set; }
	public bool IsActive { get; set; }
	public bool IsVerified { get; set; }
	public DateTime? SubscribedAtUtc { get; set; }
	public DateTime? UnsubscribedAtUtc { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}