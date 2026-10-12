namespace GhahveAlizade.Application.Features.Content.Dtos;

public class BlogTagDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int PostCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
}

public class BlogTagListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public int PostCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}