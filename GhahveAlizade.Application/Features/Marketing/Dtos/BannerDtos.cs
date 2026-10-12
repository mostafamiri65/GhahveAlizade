using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Marketing.Dtos;

public class BannerDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Subtitle { get; set; }
	public string? Description { get; set; }
	public string ImageUrl { get; set; } = string.Empty;
	public string? MobileImageUrl { get; set; }
	public string? AltText { get; set; }
	public string? LinkUrl { get; set; }
	public string? ButtonText { get; set; }
	public string? Target { get; set; }
	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public int ClickCount { get; set; }
	public int ViewCount { get; set; }
	public List<BannerPlacementDto> Placements { get; set; } = new();
	public DateTime CreatedAtUtc { get; set; }
}

public class BannerPlacementDto
{
	public long Id { get; set; }
	public BannerPlacementType Placement { get; set; }
	public string? PlacementName { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; }
}

public class BannerListItemDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	public string? LinkUrl { get; set; }
	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public int ClickCount { get; set; }
	public int ViewCount { get; set; }
	public int PlacementCount { get; set; }
	public List<string> PlacementNames { get; set; } = new();
	public DateTime CreatedAtUtc { get; set; }
}

public class BannerFormDataDto
{
	public List<PlacementLookupItem> PlacementTypes { get; set; } = new();
}

public class PlacementLookupItem
{
	public int Value { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Group { get; set; } = string.Empty;
}