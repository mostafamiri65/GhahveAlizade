using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class FlavorNoteConfiguration : BaseEntityConfiguration<FlavorNote>
{
	protected override void ConfigureEntity(EntityTypeBuilder<FlavorNote> builder)
	{
		builder.ToTable("FlavorNotes");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Category)
			.HasMaxLength(100);

		builder.Property(x => x.IconUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.ColorCode)
			.HasMaxLength(20);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_FlavorNotes_Slug");

		builder.HasIndex(x => new { x.IsActive, x.Category })
			.HasDatabaseName("IX_FlavorNotes_Active_Category");
	}
}