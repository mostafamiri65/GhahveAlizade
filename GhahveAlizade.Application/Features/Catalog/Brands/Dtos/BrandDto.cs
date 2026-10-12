namespace GhahveAlizade.Application.Features.Catalog.Brands.Dtos;

public class BrandDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? LogoUrl { get; set; }
	public string? WebsiteUrl { get; set; }
	public string? CountryOfOrigin { get; set; }

	public bool IsActive { get; set; }
	public bool IsFeatured { get; set; }
	public int DisplayOrder { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }

	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
	public int ProductCount { get; set; }
}