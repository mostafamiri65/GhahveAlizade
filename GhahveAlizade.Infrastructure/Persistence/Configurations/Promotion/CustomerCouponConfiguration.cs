using GhahveAlizade.Domain.Promotion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Promotion;

public class CustomerCouponConfiguration : BaseEntityConfiguration<CustomerCoupon>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CustomerCoupon> builder)
	{
		builder.ToTable("CustomerCoupons");

		builder.HasIndex(x => new { x.CustomerId, x.CouponId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_CustomerCoupons_Customer_Coupon");

		builder.HasIndex(x => new { x.CustomerId, x.IsUsed })
			.HasDatabaseName("IX_CustomerCoupons_Customer_Used");

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Coupon)
			.WithMany()
			.HasForeignKey(x => x.CouponId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}