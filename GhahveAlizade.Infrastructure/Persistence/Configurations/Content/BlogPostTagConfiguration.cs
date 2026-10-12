using GhahveAlizade.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Content;

public class BlogPostTagConfiguration : BaseEntityConfiguration<BlogPostTag>
{
	protected override void ConfigureEntity(EntityTypeBuilder<BlogPostTag> builder)
	{
		builder.ToTable("BlogPostTags");

		builder.HasIndex(x => new { x.BlogPostId, x.BlogTagId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_BlogPostTags_Post_Tag");

		builder.HasOne(x => x.BlogPost)
			.WithMany(p => p.PostTags)
			.HasForeignKey(x => x.BlogPostId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.BlogTag)
			.WithMany(t => t.PostTags)
			.HasForeignKey(x => x.BlogTagId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}