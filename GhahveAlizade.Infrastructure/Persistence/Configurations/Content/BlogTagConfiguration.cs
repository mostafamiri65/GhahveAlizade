using GhahveAlizade.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Content;

public class BlogTagConfiguration : BaseEntityConfiguration<BlogTag>
{
	protected override void ConfigureEntity(EntityTypeBuilder<BlogTag> builder)
	{
		builder.ToTable("BlogTags");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Description).HasMaxLength(1000);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_BlogTags_Slug");
	}
}