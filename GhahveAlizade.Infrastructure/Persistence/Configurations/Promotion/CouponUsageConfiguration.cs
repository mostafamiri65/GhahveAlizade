using GhahveAlizade.Domain.Promotion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Promotion;

public class CouponUsageConfiguration : BaseEntityConfiguration<CouponUsage>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CouponUsage> builder)
	{
		builder.ToTable("CouponUsages");

		builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");

		builder.HasIndex(x => new { x.CouponId, x.CustomerId })
			.HasDatabaseName("IX_CouponUsages_Coupon_Customer");

		builder.HasIndex(x => x.OrderId)
			.HasDatabaseName("IX_CouponUsages_OrderId");

		builder.HasOne(x => x.Coupon)
			.WithMany(c => c.Usages)
			.HasForeignKey(x => x.CouponId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Order)
			.WithMany()
			.HasForeignKey(x => x.OrderId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}