namespace GhahveAlizade.Application.Features.Content.Dtos;

public class BlogCategoryDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }
	public long? ParentId { get; set; }
	public string? ParentName { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; }
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? CanonicalUrl { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
	public int PostCount { get; set; }
	public int ChildrenCount { get; set; }
}

public class BlogCategoryListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? ParentName { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; }
	public int PostCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class BlogCategoryTreeItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public long? ParentId { get; set; }
	public int Depth { get; set; }
	public string DisplayName { get; set; } = string.Empty;
}