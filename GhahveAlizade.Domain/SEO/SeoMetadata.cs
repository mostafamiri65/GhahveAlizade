using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.SEO;

public class SeoMetadata : BaseEntity
{
	public SeoEntityType EntityType { get; set; }
	public long EntityId { get; set; }

	public string Culture { get; set; } = "fa";

	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }

	public string? OgTitle { get; set; }
	public string? OgDescription { get; set; }
	public string? OgImage { get; set; }
	public string? OgType { get; set; }

	public string? TwitterCard { get; set; }
	public string? TwitterTitle { get; set; }
	public string? TwitterDescription { get; set; }
	public string? TwitterImage { get; set; }

	public RobotsDirective Robots { get; set; } = RobotsDirective.IndexFollow;

	public string? JsonLdSchema { get; set; }

	public string? FocusKeyword { get; set; }
	public int? SeoScore { get; set; }
}