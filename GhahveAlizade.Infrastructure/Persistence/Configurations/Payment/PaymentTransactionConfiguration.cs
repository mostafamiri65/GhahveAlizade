using GhahveAlizade.Domain.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Payment;

public class PaymentTransactionConfiguration : BaseEntityConfiguration<PaymentTransaction>
{
	protected override void ConfigureEntity(EntityTypeBuilder<PaymentTransaction> builder)
	{
		builder.ToTable("PaymentTransactions");

		builder.Property(x => x.TransactionType)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Amount)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.RequestPayload).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ResponsePayload).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ErrorMessage).HasMaxLength(2000);

		builder.HasIndex(x => new { x.PaymentId, x.OccurredAtUtc })
			.HasDatabaseName("IX_PaymentTransactions_Payment_Date");

		builder.HasOne(x => x.Payment)
			.WithMany(p => p.Transactions)
			.HasForeignKey(x => x.PaymentId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}