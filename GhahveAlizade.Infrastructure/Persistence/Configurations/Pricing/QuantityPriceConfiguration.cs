using GhahveAlizade.Domain.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Pricing;

public class QuantityPriceConfiguration : BaseEntityConfiguration<QuantityPrice>
{
	protected override void ConfigureEntity(EntityTypeBuilder<QuantityPrice> builder)
	{
		builder.ToTable("QuantityPrices");

		builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
		builder.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");

		builder.HasIndex(x => new { x.ProductId, x.ProductVariantId, x.MinQuantity })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_QuantityPrices_Product_Variant_Min");

		builder.HasOne(x => x.Product)
			.WithMany()
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.ProductVariant)
			.WithMany()
			.HasForeignKey(x => x.ProductVariantId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}