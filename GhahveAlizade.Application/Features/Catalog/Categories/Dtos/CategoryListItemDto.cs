namespace GhahveAlizade.Application.Features.Catalog.Categories.Dtos;

/// <summary>
/// DTO سبک برای نمایش در لیست.
/// </summary>
public class CategoryListItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string? ImageUrl { get; set; }
	public long? ParentId { get; set; }
	public string? ParentName { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; }
	public bool IsFeatured { get; set; }
	public int ProductCount { get; set; }
	public int ChildrenCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}