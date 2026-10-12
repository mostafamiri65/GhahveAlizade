namespace GhahveAlizade.Application.Features.Content.Dtos;

public class BlogPostDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Summary { get; set; }
	public string Content { get; set; } = string.Empty;
	public string? FeaturedImageUrl { get; set; }
	public string? FeaturedImageAlt { get; set; }

	public long? CategoryId { get; set; }
	public string? CategoryName { get; set; }

	public string? AuthorId { get; set; }
	public string? AuthorName { get; set; }

	public bool IsPublished { get; set; }
	public DateTime? PublishedAtUtc { get; set; }
	public bool IsFeatured { get; set; }
	public bool AllowComments { get; set; }
	public int ViewCount { get; set; }
	public int ReadingTimeMinutes { get; set; }

	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public string? JsonLdSchema { get; set; }

	public List<long> TagIds { get; set; } = new();
	public List<string> TagNames { get; set; } = new();

	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class BlogPostListItemDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Summary { get; set; }
	public string? FeaturedImageUrl { get; set; }
	public string? CategoryName { get; set; }
	public string? AuthorName { get; set; }
	public bool IsPublished { get; set; }
	public bool IsFeatured { get; set; }
	public DateTime? PublishedAtUtc { get; set; }
	public int ViewCount { get; set; }
	public int ReadingTimeMinutes { get; set; }
	public int TagCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class BlogPostFormDataDto
{
	public List<LookupItemDto> Categories { get; set; } = new();
	public List<LookupItemDto> Tags { get; set; } = new();
}

public class LookupItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
}