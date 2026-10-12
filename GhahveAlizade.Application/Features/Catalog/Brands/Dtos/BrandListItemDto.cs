namespace GhahveAlizade.Application.Features.Catalog.Brands.Dtos;

public class BrandListItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int ProductCount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}