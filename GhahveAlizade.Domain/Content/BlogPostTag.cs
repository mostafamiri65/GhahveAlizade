using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Content;

public class BlogPostTag : BaseEntity
{
	public long BlogPostId { get; set; }
	public BlogPost BlogPost { get; set; } = default!;

	public long BlogTagId { get; set; }
	public BlogTag BlogTag { get; set; } = default!;
}