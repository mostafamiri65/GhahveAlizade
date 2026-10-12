using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Seo.Dtos;

public class SeoMetadataDto
{
	public long Id { get; set; }
	public SeoEntityType EntityType { get; set; }
	public string? EntityTypeName { get; set; }
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

	public RobotsDirective Robots { get; set; }
	public string? RobotsName { get; set; }

	public string? JsonLdSchema { get; set; }
	public string? FocusKeyword { get; set; }
	public int? SeoScore { get; set; }

	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class SeoMetadataListItemDto
{
	public long Id { get; set; }
	public SeoEntityType EntityType { get; set; }
	public string? EntityTypeName { get; set; }
	public long EntityId { get; set; }
	public string Culture { get; set; } = "fa";
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public RobotsDirective Robots { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}