using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Catalog;

public class Brand : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public string? LogoUrl { get; set; }
	public string? WebsiteUrl { get; set; }
	public string? CountryOfOrigin { get; set; }

	public bool IsActive { get; set; } = true;
	public bool IsFeatured { get; set; }
	public int DisplayOrder { get; set; }

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaKeywords { get; set; }
	public string? CanonicalUrl { get; set; }
	public string? OgImage { get; set; }

	public ICollection<Product> Products { get; set; } = new List<Product>();
}