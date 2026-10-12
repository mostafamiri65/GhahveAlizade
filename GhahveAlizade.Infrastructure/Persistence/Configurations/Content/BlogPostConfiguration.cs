using GhahveAlizade.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Content;

public class BlogPostConfiguration : BaseEntityConfiguration<BlogPost>
{
	protected override void ConfigureEntity(EntityTypeBuilder<BlogPost> builder)
	{
		builder.ToTable("BlogPosts");

		builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
		builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
		builder.Property(x => x.Summary).HasMaxLength(2000);
		builder.Property(x => x.Content).IsRequired().HasColumnType("nvarchar(max)");

		builder.Property(x => x.FeaturedImageUrl).HasMaxLength(1000);
		builder.Property(x => x.FeaturedImageAlt).HasMaxLength(500);
		builder.Property(x => x.AuthorId).HasMaxLength(450);
		builder.Property(x => x.AuthorName).HasMaxLength(200);

		builder.Property(x => x.MetaTitle).HasMaxLength(200);
		builder.Property(x => x.MetaDescription).HasMaxLength(500);
		builder.Property(x => x.MetaKeywords).HasMaxLength(500);
		builder.Property(x => x.CanonicalUrl).HasMaxLength(1000);
		builder.Property(x => x.OgImage).HasMaxLength(1000);
		builder.Property(x => x.JsonLdSchema).HasColumnType("nvarchar(max)");

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_BlogPosts_Slug");

		builder.HasIndex(x => new { x.IsPublished, x.PublishedAtUtc })
			.HasDatabaseName("IX_BlogPosts_Published");

		builder.HasIndex(x => new { x.CategoryId, x.IsPublished })
			.HasDatabaseName("IX_BlogPosts_Category_Published");

		builder.HasOne(x => x.Category)
			.WithMany(c => c.Posts)
			.HasForeignKey(x => x.CategoryId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}