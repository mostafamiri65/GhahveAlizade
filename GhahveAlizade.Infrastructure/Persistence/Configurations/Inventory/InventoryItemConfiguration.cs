using GhahveAlizade.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Inventory;

public class InventoryItemConfiguration : BaseEntityConfiguration<InventoryItem>
{
	protected override void ConfigureEntity(EntityTypeBuilder<InventoryItem> builder)
	{
		builder.ToTable("InventoryItems");

		builder.Property(x => x.Quantity)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.ReservedQuantity)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.ReorderLevel)
			.HasColumnType("decimal(18,3)");

		//builder.Ignore(x => x.AvailableQuantity);

		builder.HasIndex(x => new { x.ProductId, x.ProductVariantId, x.WarehouseId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_InventoryItems_Product_Variant_Warehouse");

		builder.HasIndex(x => new { x.WarehouseId, x.Quantity })
			.HasDatabaseName("IX_InventoryItems_Warehouse_Quantity");

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