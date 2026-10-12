using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductImage> builder)
	{
		builder.ToTable("ProductImages");

		builder.Property(x => x.ImageUrl)
			.IsRequired()
			.HasMaxLength(1000);

		builder.Property(x => x.ThumbnailUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.AltText)
			.HasMaxLength(500);

		builder.Property(x => x.Title)
			.HasMaxLength(500);

		builder.HasIndex(x => new { x.ProductId, x.DisplayOrder })
			.HasDatabaseName("IX_ProductImages_Product_Order");

		builder.HasOne(x => x.Product)
			.WithMany(p => p.Images)
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}