using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class AttributeValueConfiguration : BaseEntityConfiguration<AttributeValue>
{
	protected override void ConfigureEntity(EntityTypeBuilder<AttributeValue> builder)
	{
		builder.ToTable("AttributeValues");

		builder.Property(x => x.Value)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.DisplayValue)
			.HasMaxLength(500);

		builder.Property(x => x.ColorCode)
			.HasMaxLength(20);

		builder.HasIndex(x => new { x.AttributeId, x.DisplayOrder })
			.HasDatabaseName("IX_AttributeValues_Attribute_Order");

		builder.HasOne(x => x.Attribute)
			.WithMany(a => a.Values)
			.HasForeignKey(x => x.AttributeId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}