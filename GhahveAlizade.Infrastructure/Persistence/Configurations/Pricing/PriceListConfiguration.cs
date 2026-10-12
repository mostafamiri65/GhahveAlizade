using GhahveAlizade.Domain.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Pricing;

public class PriceListConfiguration : BaseEntityConfiguration<PriceList>
{
	protected override void ConfigureEntity(EntityTypeBuilder<PriceList> builder)
	{
		builder.ToTable("PriceLists");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Description).HasMaxLength(1000);
		builder.Property(x => x.Currency).IsRequired().HasMaxLength(10).HasDefaultValue("IRT");

		builder.HasIndex(x => x.Name)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_PriceLists_Name");

		builder.HasIndex(x => new { x.IsActive, x.IsDefault })
			.HasDatabaseName("IX_PriceLists_Active");
	}
}