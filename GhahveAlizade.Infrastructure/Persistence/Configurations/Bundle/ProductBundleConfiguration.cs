using GhahveAlizade.Domain.Bundle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Bundle;

public class ProductBundleConfiguration : BaseEntityConfiguration<ProductBundle>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductBundle> builder)
	{
		builder.ToTable("ProductBundles");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
		builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
		builder.Property(x => x.Description).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ShortDescription).HasMaxLength(1000);
		builder.Property(x => x.ImageUrl).HasMaxLength(1000);

		builder.Property(x => x.BundlePrice).HasColumnType("decimal(18,2)");
		builder.Property(x => x.CompareAtPrice).HasColumnType("decimal(18,2)");
		builder.Property(x => x.TotalItemsPrice).HasColumnType("decimal(18,2)");

		builder.Property(x => x.MetaTitle).HasMaxLength(200);
		builder.Property(x => x.MetaDescription).HasMaxLength(500);
		builder.Property(x => x.CanonicalUrl).HasMaxLength(1000);
		builder.Property(x => x.OgImage).HasMaxLength(1000);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_ProductBundles_Slug");

		builder.HasIndex(x => new { x.IsActive, x.IsFeatured })
			.HasDatabaseName("IX_ProductBundles_Active_Featured");
	}
}