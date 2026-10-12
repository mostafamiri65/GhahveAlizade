using GhahveAlizade.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Inventory;

public class StockReservationConfiguration : BaseEntityConfiguration<StockReservation>
{
	protected override void ConfigureEntity(EntityTypeBuilder<StockReservation> builder)
	{
		builder.ToTable("StockReservations");

		builder.Property(x => x.Quantity)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.ReferenceType)
			.IsRequired()
			.HasMaxLength(50);

		builder.HasIndex(x => new { x.ReferenceType, x.ReferenceId })
			.HasDatabaseName("IX_StockReservations_Reference");

		builder.HasIndex(x => new { x.ExpiresAtUtc, x.IsReleased })
			.HasDatabaseName("IX_StockReservations_Expiry");

		builder.HasOne(x => x.Product)
			.WithMany()
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.ProductVariant)
			.WithMany()
			.HasForeignKey(x => x.ProductVariantId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Warehouse)
			.WithMany()
			.HasForeignKey(x => x.WarehouseId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}