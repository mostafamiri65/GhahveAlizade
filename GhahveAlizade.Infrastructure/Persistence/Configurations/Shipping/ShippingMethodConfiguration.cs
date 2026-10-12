using GhahveAlizade.Domain.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Shipping;

public class ShippingMethodConfiguration : BaseEntityConfiguration<ShippingMethod>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ShippingMethod> builder)
	{
		builder.ToTable("ShippingMethods");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Description).HasMaxLength(1000);
		builder.Property(x => x.Code).HasMaxLength(50);
		builder.Property(x => x.ImageUrl).HasMaxLength(1000);

		builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
		builder.Property(x => x.FreeShippingThreshold).HasColumnType("decimal(18,2)");

		builder.HasIndex(x => x.Code)
			.IsUnique()
			.HasFilter("[Code] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("UX_ShippingMethods_Code");

		builder.HasIndex(x => new { x.IsActive, x.DisplayOrder })
			.HasDatabaseName("IX_ShippingMethods_Active_Order");
	}
}