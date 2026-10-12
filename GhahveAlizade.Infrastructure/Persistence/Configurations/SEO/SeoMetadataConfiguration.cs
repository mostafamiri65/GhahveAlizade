using GhahveAlizade.Domain.SEO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.SEO;

public class SeoMetadataConfiguration : BaseEntityConfiguration<SeoMetadata>
{
	protected override void ConfigureEntity(EntityTypeBuilder<SeoMetadata> builder)
	{
		builder.ToTable("SeoMetadata");

		builder.Property(x => x.Culture).IsRequired().HasMaxLength(10);
		builder.Property(x => x.MetaTitle).HasMaxLength(200);
		builder.Property(x => x.MetaDescription).HasMaxLength(500);
		builder.Property(x => x.MetaKeywords).HasMaxLength(500);
		builder.Property(x => x.CanonicalUrl).HasMaxLength(1000);
		builder.Property(x => x.OgTitle).HasMaxLength(200);
		builder.Property(x => x.OgDescription).HasMaxLength(500);
		builder.Property(x => x.OgImage).HasMaxLength(1000);
		builder.Property(x => x.OgType).HasMaxLength(50);
		builder.Property(x => x.TwitterCard).HasMaxLength(50);
		builder.Property(x => x.TwitterTitle).HasMaxLength(200);
		builder.Property(x => x.TwitterDescription).HasMaxLength(500);
		builder.Property(x => x.TwitterImage).HasMaxLength(1000);
		builder.Property(x => x.FocusKeyword).HasMaxLength(200);
		builder.Property(x => x.JsonLdSchema).HasColumnType("nvarchar(max)");

		builder.HasIndex(x => new { x.EntityType, x.EntityId, x.Culture })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_SeoMetadata_Entity_Culture");
	}
}