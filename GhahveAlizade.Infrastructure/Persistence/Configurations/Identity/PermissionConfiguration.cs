using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Identity;

public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Permission> builder)
	{
		builder.ToTable("Permissions");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(300);
		builder.Property(x => x.Description).HasMaxLength(1000);
		builder.Property(x => x.Group).HasMaxLength(100);

		builder.HasIndex(x => x.Name)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Permissions_Name");

		builder.HasIndex(x => new { x.Group, x.DisplayOrder })
			.HasDatabaseName("IX_Permissions_Group_Order");
	}
}