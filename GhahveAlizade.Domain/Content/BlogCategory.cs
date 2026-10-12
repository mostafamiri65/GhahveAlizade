using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Content;

public class BlogCategory : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }

	public long? ParentId { get; set; }
	public BlogCategory? Parent { get; set; }
	public ICollection<BlogCategory> Children { get; set; } = new List<BlogCategory>();

	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; } = true;

	// SEO
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? CanonicalUrl { get; set; }

	public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
}