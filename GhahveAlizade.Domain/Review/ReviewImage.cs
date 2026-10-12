using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Review;

public class ReviewImage : BaseEntity
{
	public long ProductReviewId { get; set; }
	public ProductReview ProductReview { get; set; } = default!;

	public string ImageUrl { get; set; } = default!;
	public string? AltText { get; set; }
	public int DisplayOrder { get; set; }
}