using GhahveAlizade.Domain.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Shipping;

public class ShipmentConfiguration : BaseEntityConfiguration<Shipment>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Shipment> builder)
	{
		builder.ToTable("Shipments");

		builder.Property(x => x.ShipmentNumber)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.CarrierName).HasMaxLength(200);
		builder.Property(x => x.TrackingNumber).HasMaxLength(200);
		builder.Property(x => x.TrackingUrl).HasMaxLength(1000);
		builder.Property(x => x.Notes).HasMaxLength(2000);

		builder.HasIndex(x => x.ShipmentNumber)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Shipments_ShipmentNumber");

		builder.HasIndex(x => x.OrderId)
			.HasDatabaseName("IX_Shipments_OrderId");

		builder.HasIndex(x => new { x.Status, x.CreatedAtUtc })
			.HasDatabaseName("IX_Shipments_Status_Date");

		builder.HasIndex(x => x.TrackingNumber)
			.HasFilter("[TrackingNumber] IS NOT NULL")
			.HasDatabaseName("IX_Shipments_TrackingNumber");

		// ⬅️ relationship رو این‌جا تعریف نکن! توی OrderConfiguration تعریف شده

		builder.HasOne(x => x.ShippingMethod)
			.WithMany(s => s.Shipments)
			.HasForeignKey(x => x.ShippingMethodId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}