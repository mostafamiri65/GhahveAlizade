using GhahveAlizade.Domain.Wholesale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Wholesale;

public class CustomerGroupConfiguration : BaseEntityConfiguration<CustomerGroup>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CustomerGroup> builder)
	{
		builder.ToTable("CustomerGroups");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Description).HasMaxLength(1000);
		builder.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");

		builder.HasIndex(x => x.Name)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_CustomerGroups_Name");

		builder.HasOne(x => x.PriceList)
			.WithMany()
			.HasForeignKey(x => x.PriceListId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}