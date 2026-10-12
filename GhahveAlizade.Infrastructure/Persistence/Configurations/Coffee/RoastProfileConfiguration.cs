using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class RoastProfileConfiguration : BaseEntityConfiguration<RoastProfile>
{
	protected override void ConfigureEntity(EntityTypeBuilder<RoastProfile> builder)
	{
		builder.ToTable("RoastProfiles");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.ColorCode)
			.HasMaxLength(20);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_RoastProfiles_Slug");

		builder.HasIndex(x => new { x.IsActive, x.DisplayOrder })
			.HasDatabaseName("IX_RoastProfiles_Active_Order");
	}
}