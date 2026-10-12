using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Content;

public class BlogPost : BaseEntity
{
	public string Title { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Summary { get; set; }
	public string Content { get; set; } = default!;

	public string? FeaturedImageUrl { get; set; }
	public string? FeaturedImageAlt { get; set; }

	public long? CategoryId { get; set; }
	public BlogCategory? Category { get; set; }

	public string? AuthorId { get; set; }
	public string? AuthorName { get; set; }

	public bool IsPublished { get; set; }
	public DateTime? PublishedAtUtc { get; set; }
	public bool IsFeatured { get; set; }
	public bool AllowComments { get; set; } = true;

	public int ViewCount { get; set; }
	public int ReadingTimeMinutes { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public string? JsonLdSchema { get; set; }

	public ICollection<BlogPostTag> PostTags { get; set; } = new List<BlogPostTag>();
}