using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class ProductVariantConfiguration : BaseEntityConfiguration<ProductVariant>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductVariant> builder)
	{
		builder.ToTable("ProductVariants");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.Sku)
			.HasMaxLength(100);

		builder.Property(x => x.Barcode)
			.HasMaxLength(100);

		builder.Property(x => x.AttributesJson)
			.HasColumnType("nvarchar(max)");

		builder.Property(x => x.Price)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.CompareAtPrice)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.CostPrice)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.Weight)
			.HasColumnType("decimal(10,2)");

		builder.Property(x => x.ImageUrl)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.Sku)
			.IsUnique()
			.HasFilter("[Sku] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("UX_ProductVariants_Sku");

		builder.HasIndex(x => new { x.ProductId, x.IsActive })
			.HasDatabaseName("IX_ProductVariants_Product");

		builder.HasOne(x => x.Product)
			.WithMany(p => p.Variants)
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}