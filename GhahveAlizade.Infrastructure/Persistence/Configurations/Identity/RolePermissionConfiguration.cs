using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Identity;

public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
{
	protected override void ConfigureEntity(EntityTypeBuilder<RolePermission> builder)
	{
		builder.ToTable("RolePermissions");

		builder.Property(x => x.RoleId).IsRequired().HasMaxLength(450);

		builder.HasIndex(x => new { x.RoleId, x.PermissionId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_RolePermissions_Role_Permission");

		builder.HasOne(x => x.Role)
			.WithMany()
			.HasForeignKey(x => x.RoleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Permission)
			.WithMany(p => p.RolePermissions)
			.HasForeignKey(x => x.PermissionId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}