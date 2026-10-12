using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class BatchInventoryConfiguration : BaseEntityConfiguration<BatchInventory>
{
	protected override void ConfigureEntity(EntityTypeBuilder<BatchInventory> builder)
	{
		builder.ToTable("BatchInventories");

		builder.Property(x => x.Quantity)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.ReservedQuantity)
			.HasColumnType("decimal(18,3)");

		builder.HasIndex(x => new { x.ProductBatchId, x.WarehouseId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_BatchInventories_Batch_Warehouse");

		builder.HasOne(x => x.ProductBatch)
			.WithMany()
			.HasForeignKey(x => x.ProductBatchId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Warehouse)
			.WithMany()
			.HasForeignKey(x => x.WarehouseId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}