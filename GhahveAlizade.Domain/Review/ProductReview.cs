using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Review;

public class ProductReview : BaseEntity
{
	public long ProductId { get; set; }
	public Catalog.Product Product { get; set; } = default!;

	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public string? AuthorName { get; set; }
	public string? AuthorEmail { get; set; }

	public int Rating { get; set; } // 1-5
	public string? Title { get; set; }
	public string? Content { get; set; }

	public bool IsApproved { get; set; }
	public bool IsVerifiedPurchase { get; set; }
	public DateTime? ApprovedAtUtc { get; set; }

	public int HelpfulCount { get; set; }
	public int NotHelpfulCount { get; set; }

	public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();
	public ICollection<ReviewReply> Replies { get; set; } = new List<ReviewReply>();
}