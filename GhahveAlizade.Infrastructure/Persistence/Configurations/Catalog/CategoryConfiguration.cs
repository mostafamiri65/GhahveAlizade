using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
	{
		builder.ToTable("Categories");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(300);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(300);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.ImageUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.IconUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.MetaTitle)
			.HasMaxLength(200);

		builder.Property(x => x.MetaDescription)
			.HasMaxLength(500);

		builder.Property(x => x.MetaKeywords)
			.HasMaxLength(500);

		builder.Property(x => x.CanonicalUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.OgImage)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Categories_Slug");

		builder.HasIndex(x => new { x.ParentId, x.DisplayOrder })
			.HasDatabaseName("IX_Categories_Parent_Order");

		builder.HasIndex(x => new { x.IsActive, x.IsDeleted })
			.HasDatabaseName("IX_Categories_Active");

		builder.HasOne(x => x.Parent)
			.WithMany(c => c.Children)
			.HasForeignKey(x => x.ParentId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}