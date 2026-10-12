using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPageBySlug;

public record GetSeoLandingPageBySlugQuery(string Slug) : IRequest<ErrorOr<SeoLandingPageDto>>;

public class SeoLandingPageDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? H1 { get; set; }
	public string? Content { get; set; }
	public string? FeaturedImageUrl { get; set; }
	public string? FilterCriteriaJson { get; set; }

	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }

	public DateTime? PublishedAtUtc { get; set; }
	public int ViewCount { get; set; }
}