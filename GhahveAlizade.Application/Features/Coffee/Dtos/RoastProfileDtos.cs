namespace GhahveAlizade.Application.Features.Coffee.Dtos;

public class RoastProfileDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? ColorCode { get; set; }
	public int RoastLevel { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class RoastProfileListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? ColorCode { get; set; }
	public int RoastLevel { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}