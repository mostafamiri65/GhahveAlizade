using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Content;

public class BlogTag : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string? Description { get; set; }
	public int PostCount { get; set; }

	public ICollection<BlogPostTag> PostTags { get; set; } = new List<BlogPostTag>();
}