using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class ProductBatchConfiguration : BaseEntityConfiguration<ProductBatch>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductBatch> builder)
	{
		builder.ToTable("ProductBatches");

		builder.Property(x => x.BatchNumber)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.RoastLevel)
			.HasMaxLength(100);

		builder.Property(x => x.RoasterNotes)
			.HasMaxLength(2000);

		builder.Property(x => x.TotalWeight)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.RemainingWeight)
			.HasColumnType("decimal(18,3)");

		builder.HasIndex(x => x.BatchNumber)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_ProductBatches_BatchNumber");

		builder.HasIndex(x => new { x.ProductId, x.RoastDate })
			.HasDatabaseName("IX_ProductBatches_Product_RoastDate");

		builder.HasIndex(x => new { x.IsActive, x.RemainingWeight })
			.HasDatabaseName("IX_ProductBatches_Active_Remaining");

		builder.HasOne(x => x.Product)
			.WithMany(p => p.Batches)
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}