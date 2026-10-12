using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class ProductRelationConfiguration : BaseEntityConfiguration<ProductRelation>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductRelation> builder)
	{
		builder.ToTable("ProductRelations");

		builder.Property(x => x.RelationType)
			.IsRequired()
			.HasMaxLength(50);

		builder.HasIndex(x => new { x.ProductId, x.RelationType })
			.HasDatabaseName("IX_ProductRelations_Product_Type");

		builder.HasOne(x => x.Product)
			.WithMany(p => p.Relations)
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.RelatedProduct)
			.WithMany()
			.HasForeignKey(x => x.RelatedProductId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}