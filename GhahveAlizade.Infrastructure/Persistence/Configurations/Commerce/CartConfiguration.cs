using GhahveAlizade.Domain.Commerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Commerce;

public class CartConfiguration : BaseEntityConfiguration<Cart>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Cart> builder)
	{
		builder.ToTable("Carts");

		builder.Property(x => x.UserId)
			.HasMaxLength(450);

		builder.Property(x => x.SessionId)
			.HasMaxLength(200);

		builder.Property(x => x.CouponCode)
			.HasMaxLength(100);

		builder.Property(x => x.Currency)
			.IsRequired()
			.HasMaxLength(10)
			.HasDefaultValue("IRT");

		builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
		builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.ShippingAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.Total).HasColumnType("decimal(18,2)");

		builder.HasIndex(x => x.UserId)
			.HasDatabaseName("IX_Carts_UserId");

		builder.HasIndex(x => x.SessionId)
			.HasDatabaseName("IX_Carts_SessionId");

		builder.HasIndex(x => new { x.IsActive, x.ExpiresAtUtc })
			.HasDatabaseName("IX_Carts_Active_Expiry");

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}