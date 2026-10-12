using GhahveAlizade.Domain.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Loyalty;

public class PointTransactionConfiguration : BaseEntityConfiguration<PointTransaction>
{
	protected override void ConfigureEntity(EntityTypeBuilder<PointTransaction> builder)
	{
		builder.ToTable("PointTransactions");

		builder.Property(x => x.ReferenceType).HasMaxLength(50);
		builder.Property(x => x.Description).HasMaxLength(1000);

		builder.HasIndex(x => new { x.CustomerPointAccountId, x.CreatedAtUtc })
			.HasDatabaseName("IX_PointTransactions_Account_Date");

		builder.HasIndex(x => new { x.Type, x.IsExpired })
			.HasDatabaseName("IX_PointTransactions_Type_Expired");

		builder.HasIndex(x => new { x.ReferenceType, x.ReferenceId })
			.HasDatabaseName("IX_PointTransactions_Reference");

		builder.HasOne(x => x.CustomerPointAccount)
			.WithMany(a => a.Transactions)
			.HasForeignKey(x => x.CustomerPointAccountId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}