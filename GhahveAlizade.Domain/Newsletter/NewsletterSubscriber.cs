using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Newsletter;

public class NewsletterSubscriber : BaseEntity
{
	public string Email { get; set; } = default!;
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? PhoneNumber { get; set; }

	public string? Source { get; set; } // Footer, Popup, Checkout
	public string? Tags { get; set; }

	public bool IsActive { get; set; } = true;
	public bool IsVerified { get; set; }
	public string? VerificationToken { get; set; }

	public DateTime? SubscribedAtUtc { get; set; }
	public DateTime? UnsubscribedAtUtc { get; set; }
	public string? UnsubscribeReason { get; set; }
}