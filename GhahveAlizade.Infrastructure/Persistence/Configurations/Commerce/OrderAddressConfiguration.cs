using GhahveAlizade.Domain.Commerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Commerce;

public class OrderAddressConfiguration : BaseEntityConfiguration<OrderAddress>
{
	protected override void ConfigureEntity(EntityTypeBuilder<OrderAddress> builder)
	{
		builder.ToTable("OrderAddresses");

		builder.Property(x => x.AddressType)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);
		builder.Property(x => x.LastName).IsRequired().HasMaxLength(200);
		builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
		builder.Property(x => x.Email).HasMaxLength(256);
		builder.Property(x => x.Province).IsRequired().HasMaxLength(100);
		builder.Property(x => x.City).IsRequired().HasMaxLength(100);
		builder.Property(x => x.PostalCode).IsRequired().HasMaxLength(20);
		builder.Property(x => x.AddressLine1).IsRequired().HasMaxLength(1000);
		builder.Property(x => x.AddressLine2).HasMaxLength(1000);

		builder.Property(x => x.Latitude).HasColumnType("decimal(10,7)");
		builder.Property(x => x.Longitude).HasColumnType("decimal(10,7)");

		builder.HasIndex(x => new { x.OrderId, x.AddressType })
			.HasDatabaseName("IX_OrderAddresses_Order_Type");

		builder.HasOne(x => x.Order)
			.WithMany(o => o.Addresses)
			.HasForeignKey(x => x.OrderId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}