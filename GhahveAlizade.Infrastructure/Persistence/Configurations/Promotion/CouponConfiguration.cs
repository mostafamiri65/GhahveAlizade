using GhahveAlizade.Domain.Promotion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Promotion;

public class CouponConfiguration : BaseEntityConfiguration<Coupon>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Coupon> builder)
	{
		builder.ToTable("Coupons");

		builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
		builder.Property(x => x.Description).HasMaxLength(1000);

		builder.Property(x => x.DiscountValue).HasColumnType("decimal(18,2)");
		builder.Property(x => x.MaxDiscountAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.MinOrderAmount).HasColumnType("decimal(18,2)");

		builder.Property(x => x.ApplicableProductIdsJson).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ApplicableCategoryIdsJson).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ApplicableBrandIdsJson).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ExcludedProductIdsJson).HasColumnType("nvarchar(max)");

		builder.HasIndex(x => x.Code)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Coupons_Code");

		builder.HasIndex(x => new { x.IsActive, x.StartsAtUtc, x.ExpiresAtUtc })
			.HasDatabaseName("IX_Coupons_Active_Date");
	}
}