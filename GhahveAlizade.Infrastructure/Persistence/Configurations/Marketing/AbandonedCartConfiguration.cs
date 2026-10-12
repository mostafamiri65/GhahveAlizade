using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class AbandonedCartConfiguration : BaseEntityConfiguration<AbandonedCart>
{
	protected override void ConfigureEntity(EntityTypeBuilder<AbandonedCart> builder)
	{
		builder.ToTable("AbandonedCarts");

		builder.Property(x => x.Email).HasMaxLength(256);
		builder.Property(x => x.PhoneNumber).HasMaxLength(20);
		builder.Property(x => x.CartTotal).HasColumnType("decimal(18,2)");

		builder.HasIndex(x => x.CartId)
			.HasDatabaseName("IX_AbandonedCarts_CartId");

		builder.HasIndex(x => new { x.IsRecovered, x.DetectedAtUtc })
			.HasDatabaseName("IX_AbandonedCarts_Recovered_Detected");

		builder.HasOne(x => x.Cart)
			.WithMany()
			.HasForeignKey(x => x.CartId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}