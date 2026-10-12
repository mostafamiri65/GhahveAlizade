namespace GhahveAlizade.Application.Features.Coffee.Dtos;

public class FlavorNoteDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Category { get; set; }
	public string? IconUrl { get; set; }
	public string? ColorCode { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public int ProductCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class FlavorNoteListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Category { get; set; }
	public string? ColorCode { get; set; }
	public bool IsActive { get; set; }
	public int DisplayOrder { get; set; }
	public int ProductCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}