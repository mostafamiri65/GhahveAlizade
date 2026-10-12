using GhahveAlizade.Domain.SEO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.SEO;

public class SeoLandingPageConfiguration : BaseEntityConfiguration<SeoLandingPage>
{
	protected override void ConfigureEntity(EntityTypeBuilder<SeoLandingPage> builder)
	{
		builder.ToTable("SeoLandingPages");

		builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
		builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
		builder.Property(x => x.H1).HasMaxLength(500);
		builder.Property(x => x.Content).HasColumnType("nvarchar(max)");
		builder.Property(x => x.FeaturedImageUrl).HasMaxLength(1000);
		builder.Property(x => x.FilterCriteriaJson).HasColumnType("nvarchar(max)");

		builder.Property(x => x.MetaTitle).HasMaxLength(200);
		builder.Property(x => x.MetaDescription).HasMaxLength(500);
		builder.Property(x => x.MetaKeywords).HasMaxLength(500);
		builder.Property(x => x.CanonicalUrl).HasMaxLength(1000);
		builder.Property(x => x.OgImage).HasMaxLength(1000);
		builder.Property(x => x.JsonLdSchema).HasColumnType("nvarchar(max)");

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_SeoLandingPages_Slug");

		builder.HasIndex(x => new { x.IsPublished, x.PublishedAtUtc })
			.HasDatabaseName("IX_SeoLandingPages_Published");
	}
}