using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Review;

public class ReviewReply : BaseEntity
{
	public long ProductReviewId { get; set; }
	public ProductReview ProductReview { get; set; } = default!;

	public string Content { get; set; } = default!;
	public string? RepliedByUserId { get; set; }
	public string? RepliedByName { get; set; }

	public bool IsOfficial { get; set; }
	public bool IsApproved { get; set; }
}