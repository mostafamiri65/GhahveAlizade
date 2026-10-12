using GhahveAlizade.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Inventory;

public class InventoryTransactionConfiguration : BaseEntityConfiguration<InventoryTransaction>
{
	protected override void ConfigureEntity(EntityTypeBuilder<InventoryTransaction> builder)
	{
		builder.ToTable("InventoryTransactions");

		builder.Property(x => x.TransactionType)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Quantity)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.BalanceAfter)
			.HasColumnType("decimal(18,3)");

		builder.Property(x => x.ReferenceType)
			.HasMaxLength(100);

		builder.Property(x => x.Notes)
			.HasMaxLength(1000);

		builder.HasIndex(x => new { x.InventoryItemId, x.CreatedAtUtc })
			.HasDatabaseName("IX_InventoryTransactions_Item_Date");

		builder.HasIndex(x => new { x.ReferenceType, x.ReferenceId })
			.HasDatabaseName("IX_InventoryTransactions_Reference");

		builder.HasOne(x => x.InventoryItem)
			.WithMany()
			.HasForeignKey(x => x.InventoryItemId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}