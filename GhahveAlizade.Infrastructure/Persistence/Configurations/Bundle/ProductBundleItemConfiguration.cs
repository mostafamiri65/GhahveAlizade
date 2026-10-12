using GhahveAlizade.Domain.Bundle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Bundle;

public class ProductBundleItemConfiguration : BaseEntityConfiguration<ProductBundleItem>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductBundleItem> builder)
	{
		builder.ToTable("ProductBundleItems");

		builder.HasIndex(x => new { x.ProductBundleId, x.ProductId, x.ProductVariantId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_ProductBundleItems_Bundle_Product_Variant");

		builder.HasOne(x => x.ProductBundle)
			.WithMany(b => b.Items)
			.HasForeignKey(x => x.ProductBundleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Product)
			.WithMany()
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.ProductVariant)
			.WithMany()
			.HasForeignKey(x => x.ProductVariantId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}