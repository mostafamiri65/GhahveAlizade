using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class ProductAttributeConfiguration : BaseEntityConfiguration<ProductAttribute>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductAttribute> builder)
	{
		builder.ToTable("ProductAttributes");

		builder.Property(x => x.CustomValue)
			.HasMaxLength(1000);

		builder.HasIndex(x => new { x.ProductId, x.AttributeId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_ProductAttributes_Product_Attribute");

		builder.HasOne(x => x.Product)
			.WithMany(p => p.Attributes)
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Attribute)
			.WithMany(a => a.ProductAttributes)
			.HasForeignKey(x => x.AttributeId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.AttributeValue)
			.WithMany()
			.HasForeignKey(x => x.AttributeValueId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}