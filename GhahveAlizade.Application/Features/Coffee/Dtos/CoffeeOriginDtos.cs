namespace GhahveAlizade.Application.Features.Coffee.Dtos;

public class CoffeeOriginDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CountryCode { get; set; }
    public string? FlagUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public int RegionCount { get; set; }
    public int ProductCount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public List<CoffeeRegionDto> Regions { get; set; } = new();
}

public class CoffeeRegionDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Altitude { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

public class CoffeeOriginListItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
    public string? FlagUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public int RegionCount { get; set; }
    public int ProductCount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}