using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.SEO;

public class SeoLandingPage : BaseEntity
{
	public string Title { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? H1 { get; set; }
	public string? Content { get; set; }

	public string? FeaturedImageUrl { get; set; }

	// فیلترهای از پیش تعریف‌شده برای نمایش محصولات
	public string? FilterCriteriaJson { get; set; }

	public bool IsPublished { get; set; }
	public DateTime? PublishedAtUtc { get; set; }
	public int ViewCount { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }
	public string? JsonLdSchema { get; set; }
}