namespace GhahveAlizade.Application.Features.Coffee.Dtos;

public class CoffeeProcessDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }
	public string? IconUrl { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class CoffeeProcessListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? ImageUrl { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}