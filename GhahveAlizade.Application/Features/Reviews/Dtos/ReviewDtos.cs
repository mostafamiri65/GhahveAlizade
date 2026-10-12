namespace GhahveAlizade.Application.Features.Reviews.Dtos;

public class ReviewListItemDto
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ProductSlug { get; set; }
	public string? CustomerName { get; set; }
	public string? AuthorName { get; set; }
	public string? AuthorEmail { get; set; }
	public int Rating { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public bool IsApproved { get; set; }
	public bool IsVerifiedPurchase { get; set; }
	public int HelpfulCount { get; set; }
	public int NotHelpfulCount { get; set; }
	public int ReplyCount { get; set; }
	public int ImageCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class ReviewDetailDto : ReviewListItemDto
{
	public List<ReviewImageDto> Images { get; set; } = new();
	public List<ReviewReplyDto> Replies { get; set; } = new();
}

public class ReviewImageDto
{
	public long Id { get; set; }
	public string ImageUrl { get; set; } = string.Empty;
	public string? AltText { get; set; }
}

public class ReviewReplyDto
{
	public long Id { get; set; }
	public string Content { get; set; } = string.Empty;
	public string? RepliedByName { get; set; }
	public bool IsOfficial { get; set; }
	public bool IsApproved { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}