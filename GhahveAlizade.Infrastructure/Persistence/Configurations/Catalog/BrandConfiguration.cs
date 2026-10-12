using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class BrandConfiguration : BaseEntityConfiguration<Brand>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Brand> builder)
	{
		builder.ToTable("Brands");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(300);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(300);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.LogoUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.WebsiteUrl)
			.HasMaxLength(500);

		builder.Property(x => x.CountryOfOrigin)
			.HasMaxLength(100);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Brands_Slug");

		builder.HasIndex(x => new { x.IsActive, x.IsFeatured })
			.HasDatabaseName("IX_Brands_Status");
	}
}