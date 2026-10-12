using GhahveAlizade.Domain.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Payment;

public class PaymentConfiguration : BaseEntityConfiguration<Domain.Payment.Payment>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Domain.Payment.Payment> builder)
	{
		builder.ToTable("Payments");

		builder.Property(x => x.PaymentNumber)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Currency)
			.IsRequired()
			.HasMaxLength(10)
			.HasDefaultValue("IRT");

		builder.Property(x => x.Amount)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.GatewayName).HasMaxLength(100);
		builder.Property(x => x.GatewayTransactionId).HasMaxLength(200);
		builder.Property(x => x.GatewayReferenceId).HasMaxLength(200);
		builder.Property(x => x.GatewayResponse).HasColumnType("nvarchar(max)");
		builder.Property(x => x.FailureReason).HasMaxLength(1000);

		builder.HasIndex(x => x.PaymentNumber)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Payments_PaymentNumber");

		builder.HasIndex(x => x.OrderId)
			.HasDatabaseName("IX_Payments_OrderId");

		builder.HasIndex(x => new { x.Status, x.CreatedAtUtc })
			.HasDatabaseName("IX_Payments_Status_Date");

		builder.HasIndex(x => x.GatewayTransactionId)
			.HasFilter("[GatewayTransactionId] IS NOT NULL")
			.HasDatabaseName("IX_Payments_GatewayTransactionId");

		// ⬅️ relationship رو این‌جا تعریف نکن! توی OrderConfiguration تعریف شده
	}
}