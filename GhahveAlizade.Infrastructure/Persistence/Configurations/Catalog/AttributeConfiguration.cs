using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class AttributeConfiguration : BaseEntityConfiguration<Domain.Catalog.Attribute>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Domain.Catalog.Attribute> builder)
	{
		builder.ToTable("Attributes");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Unit)
			.HasMaxLength(50);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Attributes_Slug");

		builder.HasIndex(x => x.IsFilterable)
			.HasDatabaseName("IX_Attributes_Filterable");
	}
}