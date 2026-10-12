using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class CoffeeRegionConfiguration : BaseEntityConfiguration<CoffeeRegion>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CoffeeRegion> builder)
	{
		builder.ToTable("CoffeeRegions");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.Altitude)
			.HasMaxLength(100);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_CoffeeRegions_Slug");

		builder.HasIndex(x => new { x.CoffeeOriginId, x.DisplayOrder })
			.HasDatabaseName("IX_CoffeeRegions_Origin_Order");

		builder.HasOne(x => x.CoffeeOrigin)
			.WithMany(o => o.Regions)
			.HasForeignKey(x => x.CoffeeOriginId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}